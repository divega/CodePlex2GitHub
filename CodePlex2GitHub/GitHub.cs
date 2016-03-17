using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Octokit;

namespace CodePlex2GitHub
{
    public class GitHub
    {
        private readonly GitHubClient _client;
        private readonly CodePlexDbContext _context;
        private readonly string _repoOwner;
        private readonly string _repoName;
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


        private Task<Issue> CreateEmptyIssue()
        {
            return _client.Issue.Create(_repoOwner, _repoName, new NewIssue("New Issue"));
        }
        private async Task<IssueUpdate> GetOrAddIssue(int number)
        {
            var existingIssue = await _client.Issue.Get(_repoOwner, _repoName, number);
            if (existingIssue != null)
            {
                return existingIssue.ToUpdate();
            }
            var newIssue = await CreateEmptyIssue();
            var issueUpdate = newIssue.ToUpdate();
            while (newIssue.Number < number)
            {
                await DeleteIssue(newIssue.Number, issueUpdate);
                newIssue = await CreateEmptyIssue();
                issueUpdate = newIssue.ToUpdate();
            }

            if (newIssue.Number > number)
            {
                throw new InvalidOperationException($"Issues out of sequence: CodePlex #{number}, GitHub #{newIssue.Number}");
            }
            return issueUpdate;
        }

        private async Task DeleteIssue(int number, IssueUpdate issueUpdate)
        {
            issueUpdate.Title = "This issue has been removed or does not exist";
            issueUpdate.State = ItemState.Closed;
            issueUpdate.Assignee = null;
            issueUpdate.Body = null;
            issueUpdate.ClearLabels();
            issueUpdate.Milestone = null;
            await UpdateIssue(number, issueUpdate);
            await RemoveIssueComments(number);

        }

        private async Task RemoveIssueComments(int number)
        {
            foreach (var comment in await _client.Issue.Comment.GetAllForIssue(_repoOwner, _repoName, number))
            {
                await _client.Issue.Comment.Delete(_repoOwner, _repoName, comment.Id);
            }
        }

        private Task UpdateIssue(int number, IssueUpdate issueUpdate)
        {
            return _client.Issue.Update(_repoOwner, _repoName, number, issueUpdate);
        }

        public async Task MigrateAllIssuesAsync()
        {
            // TODO: ForEachAsync (and FWIW MyForEachAsync) has issues when debugging
            foreach (var codePlexIssue in _context.Issues.OrderBy(i => i.Number))
            {
                await MigrateIssueAsync(codePlexIssue);
            }
        }

        public async Task MigrateIssueAsync(WorkItem codePlexWorkItem)
        {
            try
            {
                var update = await GetOrAddIssue(codePlexWorkItem.Number);
                update.Title = codePlexWorkItem.Title;
                update.Body = codePlexWorkItem.Body;
                update.Assignee = codePlexWorkItem.AssignedTo?.GitHubAlias;
                // TODO: fill in details and comments
                //update.Milestone = codePlexWorkItem.Release?.Name;
                await UpdateIssue(codePlexWorkItem.Number, update);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        public async Task MigrateReleases()
        {
            var codePlexReleases = await _context.Releases.Where(p => !p.IsInvestigation).OrderBy(p => p.GitHubName).ToListAsync();
            var gitHubMilestones = await _client.Issue.Milestone.GetAllForRepository(_repoOwner, _repoName);
            foreach (var codePlexRelease in codePlexReleases)
            {
                var gitHubMilestone = gitHubMilestones.SingleOrDefault(m => m.Title == codePlexRelease.GitHubName) ??
                                      await _client.Issue.Milestone.Create(_repoOwner, _repoName,
                                            new NewMilestone(codePlexRelease.GitHubName));

                var milestoneUpdate = new MilestoneUpdate
                {
                    Title = gitHubMilestone.Title,
                    Description = codePlexRelease.Body,
                    State = codePlexRelease.IsReleased?ItemState.Closed : ItemState.Open,
                    DueOn = codePlexRelease.ReleaseDate,
                };

                await _client.Issue.Milestone.Update(_repoOwner, _repoName, gitHubMilestone.Number, milestoneUpdate);

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

