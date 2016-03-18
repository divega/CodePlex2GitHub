using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Octokit;
using Remotion.Linq.Clauses;

namespace CodePlex2GitHub
{
    public class GitHub
    {
        private readonly GitHubClient _client;
        private readonly CodePlexDbContext _context;
        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly Dictionary<string, Milestone> MilestoneMap = new Dictionary<string, Milestone>();

        public GitHub(string authToken, string repoOwner, string repoName, CodePlexDbContext context)
        {
            _context = context;
            _repoName = repoName;
            _repoOwner = repoOwner;
            _client = new GitHubClient(new ProductHeaderValue("CodePlex2GitHub"))
            {
                Credentials = new Credentials(authToken)
            };
        }


        private Task<Issue> CreateEmptyIssueAsync()
        {
            return _client.Issue.Create(_repoOwner, _repoName, new NewIssue("New Issue"));
        }

        private async Task<IssueUpdate> GetOrAddIssueAsync(int number)
        {
            var existingIssue = await _client.Issue.Get(_repoOwner, _repoName, number);
            if (existingIssue != null)
            {
                return existingIssue.ToUpdate();
            }
            var newIssue = await CreateEmptyIssueAsync();
            var issueUpdate = newIssue.ToUpdate();
            while (newIssue.Number < number)
            {
                await DeleteIssueAsync(newIssue.Number, issueUpdate);
                newIssue = await CreateEmptyIssueAsync();
                issueUpdate = newIssue.ToUpdate();
            }

            if (newIssue.Number > number)
            {
                throw new InvalidOperationException($"Issues out of sequence: CodePlex #{number}, GitHub #{newIssue.Number}");
            }
            return issueUpdate;
        }

        private async Task DeleteIssueAsync(int number, IssueUpdate issueUpdate)
        {
            issueUpdate.Title = "This issue has been deleted";
            issueUpdate.State = ItemState.Closed;
            issueUpdate.Assignee = null;
            issueUpdate.Body = null;
            issueUpdate.ClearLabels();
            issueUpdate.Milestone = null;
            await UpdateIssueAsync(number, issueUpdate);
            await DeleteIssueCommentsAsync(number);

        }

        private async Task DeleteIssueCommentsAsync(int number)
        {
            foreach (var comment in await _client.Issue.Comment.GetAllForIssue(_repoOwner, _repoName, number))
            {
                await _client.Issue.Comment.Delete(_repoOwner, _repoName, comment.Id);
            }
        }

        private Task UpdateIssueAsync(int number, IssueUpdate issueUpdate)
        {
            return _client.Issue.Update(_repoOwner, _repoName, number, issueUpdate);
        }

        public async Task MigrateAllIssuesAsync()
        {
            // TODO: Bug: ForEachAsync has issues when debugging
            foreach (var codePlexIssue in _context.GetWorkItemAggregates())
            {
                await MigrateIssueAsync(codePlexIssue);
            }
        }

        public async Task MigrateIssueComments(WorkItem codePlexWorkItem)
        {
            await DeleteIssueCommentsAsync(codePlexWorkItem.Number); 
            foreach (var comment in codePlexWorkItem.Comments.OrderBy(c => c.PostedOn))
            {
                await _client.Issue.Comment.Create(_repoOwner, _repoName, codePlexWorkItem.Number, comment.Body);
                // TODO: Add comment metadata

            }
        }

        public async Task MigrateIssueAsync(WorkItem codePlexWorkItem)
        {
            try
            {
                var update = await GetOrAddIssueAsync(codePlexWorkItem.Number);
                update.Title = codePlexWorkItem.Title;
                update.Body = codePlexWorkItem.Body;
                update.Assignee = codePlexWorkItem.AssignedTo?.GitHubAlias;
                // TODO: fill in metadata
                if (codePlexWorkItem.Release != null)
                {
                    update.Milestone = MilestoneMap[codePlexWorkItem.Release.GitHubName].Number;
                }
                await UpdateIssueAsync(codePlexWorkItem.Number, update);
                await MigrateIssueComments(codePlexWorkItem);
                await MigrateIssueLabels(codePlexWorkItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        private async Task MigrateIssueLabels(WorkItem codePlexWorkItem)
        {
            var issueLabels = new HashSet<string>()
                .AddIf(codePlexWorkItem.Title.Contains("UpForGrabs"), "up-for-grabs")
                .AddIf(codePlexWorkItem.Type == WorkItem.WorkItemType.Issue, "bug")
                .AddIf(codePlexWorkItem.Type == WorkItem.WorkItemType.Feature, "enhancement")
                .AddIf(codePlexWorkItem.Type == WorkItem.WorkItemType.Task, "task")
                .AddIf(codePlexWorkItem.Impact == WorkItem.WorkItemImpact.High, "high-impact")
                .AddIf(codePlexWorkItem.Impact == WorkItem.WorkItemImpact.Low, "low-impact")
                .AddIf(codePlexWorkItem.Release?.IsInvestigation == true, "investigation-needed")
                .AddIf(codePlexWorkItem.Title.Contains("[Performance]"), "perf")
                .AddIf(codePlexWorkItem.Title.Contains("[UX]"), "designer")
                .AddIf(codePlexWorkItem.Title.Contains("[Migration]"), "migrations")
                .AddIf(codePlexWorkItem.Title.Contains("regression"), "regression")
                .AddIf(codePlexWorkItem.Status == WorkItem.WorkItemStatus.Active, "working")
                .AddIf(codePlexWorkItem.Votes >= 200, "200+votes")
                .AddIf(codePlexWorkItem.Votes < 200 && codePlexWorkItem.Votes >= 100, "100+votes")
                .AddIf(codePlexWorkItem.Votes < 100 && codePlexWorkItem.Votes >= 50, "50+votes")
                .AddIf(codePlexWorkItem.Votes < 50 && codePlexWorkItem.Votes >= 20, "20+votes")
                .AddIf(codePlexWorkItem.Votes < 20 && codePlexWorkItem.Votes >= 10, "10+votes");

            issueLabels.Add(codePlexWorkItem.Component?.GitHubName);
            issueLabels.Add(codePlexWorkItem.ClosingReason?.GitHubName);
            issueLabels.Remove(null);

            await _client.Issue.Labels.ReplaceAllForIssue(_repoOwner, _repoName, codePlexWorkItem.Number, issueLabels.ToArray());
        }


        public async Task MigrateReleasesAsync()
        {
            var codePlexReleases = await _context.Releases.Where(p => !p.IsInvestigation).OrderBy(p => p.GitHubName).ToListAsync();
            var gitHubMilestones = await _client.Issue.Milestone.GetAllForRepository(_repoOwner, _repoName, new MilestoneRequest { State = ItemState.All });
            foreach (var codePlexRelease in codePlexReleases)
            {
                var gitHubMilestone = gitHubMilestones.SingleOrDefault(m => m.Title == codePlexRelease.GitHubName) ??
                                      await _client.Issue.Milestone.Create(_repoOwner, _repoName,
                                            new NewMilestone(codePlexRelease.GitHubName));

                var milestoneUpdate = new MilestoneUpdate
                {
                    Title = gitHubMilestone.Title,
                    Description = codePlexRelease.Body,
                    State = codePlexRelease.IsReleased ? ItemState.Closed : ItemState.Open,
                    DueOn = codePlexRelease.ReleaseDate,
                };

                var updatedMilestone = await _client.Issue.Milestone.Update(_repoOwner, _repoName, gitHubMilestone.Number, milestoneUpdate);
                MilestoneMap[updatedMilestone.Title] = updatedMilestone;
            }
        }

        public async Task MigrateRepoLabelsAsync()
        {
            await DeleteAllNonStandardLabelsAsync();
            await CreateLabelAsync("investigation-needed", "0052cc");
            await CreateLabelAsync("task", "d4c5f9");
            await CreateLabelAsync("perf", "fef2c0");
            await CreateLabelAsync("test", "c7def8");
            await CreateLabelAsync("up-for-grabs", "0090FF");
            await CreateLabelAsync("pri0", "e11d21");
            await CreateLabelAsync("pri1", "eb6420");
            await CreateLabelAsync("needs-review", "e11d21");
            await CreateLabelAsync("providers-beware", "fbca04");
            await CreateLabelAsync("cleanup", "bfdadc");
            await CreateLabelAsync("blocked", "e11d21");
            await CreateLabelAsync("working", "cccccc");
            await CreateLabelAsync("low-impact", "fad8c7");
            await CreateLabelAsync("high-impact", "f7c6c7");
            await CreateLabelAsync("+200votes", "5319e7");
            await CreateLabelAsync("+100votes", "5319e7");
            await CreateLabelAsync("+50votes", "5319e7");
            await CreateLabelAsync("+20votes", "5319e7");
            await CreateLabelAsync("+10votes", "5319e7");
            await CreateLabelAsync("regression", "e11d21");
            foreach (var component in _context.Components.OrderBy(c => c.Name))
            {
                await CreateLabelAsync(component.GitHubName, "c7def8");
            }
            foreach (var reason in _context.WorkItemClosingReasons.OrderBy(r => r.Name))
            {
                await CreateLabelAsync(reason.GitHubName, "006b75");
            }
        }

        public async Task CreateLabelAsync(string name, string color)
        {
            if (!_labels.Contains(name))
            {
                await _client.Issue.Labels.Create(_repoOwner, _repoName, new NewLabel(name, color));
                _labels.Add(name);
            }
        }

        private readonly HashSet<string> _labels = new HashSet<string>
        {
            "bug",
            "duplicate",
            "enhancement",
            "help wanted",
            "invalid",
            "question",
            "wontfix"
        };

        private async Task DeleteAllNonStandardLabelsAsync()
        {
            var labels = await _client.Issue.Labels.GetAllForRepository(_repoOwner, _repoName);
            foreach (var label in labels)
            {
                if (!_labels.Contains(label.Name))
                {
                    await _client.Issue.Labels.Delete(_repoOwner, _repoName, label.Name);
                }
            }
        }
    }

    //public static class QueryExtensions
    //{
    //    public async static Task MyForEachAsync<T>(this IQueryable<T> source, Action<T> action)
    //    {
    //        var enumerable = source.ToAsyncEnumerable();
    //        using (var enumerator = enumerable.GetEnumerator())
    //        {
    //            while (await enumerator.MoveNext())
    //            {
    //                action(enumerator.Current);
    //            }
    //        }
    //    }
    //}
}

