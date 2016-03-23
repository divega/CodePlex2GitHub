using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;
using Octokit;

namespace CodePlex2GitHub
{
    public class GitHub
    {
        private readonly GitHubClient _client;
        private readonly CodePlexDbContext _context;

        private readonly Dictionary<string, string> _nameMap = new Dictionary<string, string>
        {
            {"Build/Infra.", "build"},
            {"Component", "GitHubName"},
            {"Designer", "designer"},
            {"Documentation", "documentation"},
            {"Migrations", "migrations"},
            {"Power Tools", "powertools"},
            {"PowerShell", "powershell"},
            {"Runtime", "runtime"},
            {"Templates", "templates"},
            {"Tests", "test"},
            {"By Design", "bydesign"},
            {"Could Not Repro", "norepro"},
            {"Duplicate", "duplicate"},
            {"External Issue", "external"},
            {"Won’t Fix", "wontfix"},
            {"General", null},
            {"EF Runtime", "runtime"},
            {"EF Power Tools", "powertools"},
            {"EF Designer ", "designer"},
            {"Future", "Backlog"},
            {"Investigation", "Backlog"},
            {"ajcvickers", "ajcvickers"},
            {"AndrewPeters", "anpete"},
            {"AndriySvyryd", "AndriySvyryd"},
            {"brecon", "BrennanConroy"},
            {"BriceLambson", "bricelam"},
            {"DavidObando", "DavidObando"},
            {"divega", "divega"},
            {"eilonlipton", "eilon"},
            {"emilcicos", "emilcicos"},
            {"lajones", "lajones"},
            {"maumar", "maumar"},
            {"mgirgin", "muratg"},
            {"RoMiller", "rowanmiller"},
            {"smitpatel", "smitpatel"},
            {"moozzyk", "moozzyk"},
            {"lukew", "lukewaters"},
            {"EF 5.0.0", "5.0.0"},
            {"EF 6.0.0", "6.0.0"},
            {"EF 6.0.1", "6.0.1"},
            {"EF 6.0.2", "6.0.2"},
            {"EF 6.1.0", "6.1.0"},
            {"EF 6.1.1", "6.1.1"},
            {"EF 6.1.2", "6.1.2"},
            {"EF 6.1.3", "6.1.3"},
            {"EF 6.2.0", "6.2.0"},
            {"", null},
        };

        private readonly string _repoName;
        private readonly string _repoOwner;

        private Dictionary<string, Milestone> _milestoneMap = null;

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

        private string GetGitHubName(string codePlexName)
        {
            string gitHubName = null;
            return _nameMap.TryGetValue(codePlexName, out gitHubName)
                ? gitHubName
                : codePlexName;
        }

        private int? GetHitHubMilestone(string releaseName)
        {
            return (releaseName == null) ? null : _milestoneMap[GetGitHubName(releaseName)]?.Number;
        }


        private Task<Issue> CreateEmptyIssueAsync(string title = "New Issue")
        {
            return _client.Issue.Create(_repoOwner, _repoName, new NewIssue(title));
        }

        private async Task<IssueUpdate> GetOrAddIssueAsync(int number, string title)
        {
            var existingIssue = await _client.Issue.Get(_repoOwner, _repoName, number);
            if (existingIssue != null)
            {
                return existingIssue.ToUpdate();
            }
            var newIssue = await CreateEmptyIssueAsync(title);
            var issueUpdate = newIssue.ToUpdate();
            while (newIssue.Number < number)
            {
                await DeleteIssueAsync(newIssue.Number, issueUpdate);
                newIssue = await CreateEmptyIssueAsync(title);
                issueUpdate = newIssue.ToUpdate();
            }

            if (newIssue.Number > number)
            {
                throw new InvalidOperationException(
                    $"Issues out of sequence: CodePlex #{number}, GitHub #{newIssue.Number}");
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
                // TODO, is there a non-nullable FK on the attachment path?
                await MigrateIssueAsync(codePlexIssue);
            }
        }

        public async Task MigrateIssueComments(WorkItem codePlexWorkItem)
        {
            await DeleteIssueCommentsAsync(codePlexWorkItem.WorkItemId);
            foreach (var comment in codePlexWorkItem.Comments.OrderBy(c => c.Date))
            {
                await _client.Issue.Comment.Create(_repoOwner, _repoName, codePlexWorkItem.WorkItemId, comment.Comment);
                // TODO: Add comment metadata
            }
        }

        public async Task MigrateIssueAsync(WorkItem workItem)
        {
            try
            {
                var update = await GetOrAddIssueAsync(workItem.WorkItemId, workItem.Summary);
                update.Title = workItem.Summary;
                update.Body = workItem.Description;
                update.Assignee = GetGitHubName(workItem.AssignedTo?.Name);
                update.State = workItem.Status == WorkItem.WorkItemStatus.Closed
                    ? ItemState.Closed
                    : ItemState.Open;
                // TODO: fill in metadata
                update.Milestone = GetHitHubMilestone(workItem.PlannedForRelease);
                await UpdateIssueAsync(workItem.WorkItemId, update);
                await MigrateIssueComments(workItem);
                await MigrateIssueLabels(workItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task MigrateIssueLabels(WorkItem workItem)
        {
            var issueLabels = new HashSet<string>()
                .AddIf(workItem.Summary.Contains("UpForGrabs"), "up-for-grabs")
                .AddIf(workItem.Type == WorkItem.WorkItemType.Issue, "bug")
                .AddIf(workItem.Type == WorkItem.WorkItemType.Feature, "enhancement")
                .AddIf(workItem.Type == WorkItem.WorkItemType.Task, "task")
                .AddIf(workItem.Severity == WorkItem.WorkItemSeverity.High, "high-impact")
                .AddIf(workItem.Severity == WorkItem.WorkItemSeverity.Low, "low-impact")
                .AddIf(workItem.PlannedForRelease == "Investigation" == true, "investigation-needed")
                .AddIf(workItem.Summary.Contains("[Performance]"), "perf")
                .AddIf(workItem.Summary.Contains("[UX]"), "designer")
                .AddIf(workItem.Summary.Contains("[Migration]"), "migrations")
                .AddIf(workItem.Summary.Contains("regression"), "regression")
                .AddIf(workItem.Status == WorkItem.WorkItemStatus.Active, "working")
                .AddIf(workItem.Status == WorkItem.WorkItemStatus.Resolved, "fixed")
                .AddIf(workItem.VoteCount >= 200, "200+votes")
                .AddIf(workItem.VoteCount < 200 && workItem.VoteCount >= 100, "100+votes")
                .AddIf(workItem.VoteCount < 100 && workItem.VoteCount >= 50, "50+votes")
                .AddIf(workItem.VoteCount < 50 && workItem.VoteCount >= 20, "20+votes")
                .AddIf(workItem.VoteCount < 20 && workItem.VoteCount >= 10, "10+votes");

            issueLabels.Add("codeplex");
            issueLabels.Add(GetGitHubName(workItem.ReasonClosed)?.ToLower());
            issueLabels.Add(GetGitHubName(workItem.Component)?.ToLower());
            issueLabels.Remove(null);

            await
                _client.Issue.Labels.ReplaceAllForIssue(_repoOwner, _repoName, workItem.WorkItemId,
                    issueLabels.ToArray());
        }


        public async Task MigrateMilestonesAsync()
        {
            var codePlexReleases =
                _context.GetReleases().Where(p => p.Name != "Investigation")
                    .OrderBy(p => GetGitHubName(p.Name));

            _milestoneMap =
                (await
                    _client.Issue.Milestone.GetAllForRepository(_repoOwner, _repoName,
                        new MilestoneRequest {State = ItemState.All})).ToDictionary(m => m.Title);

            // we need a null milestone entry for work items that contain an empty string for release
            _milestoneMap[""] = null;

            foreach (var codePlexRelease in codePlexReleases)
            {
                Milestone gitHubMilestone = null;
                var milestoneName = GetGitHubName(codePlexRelease.Name);
                if (_milestoneMap.TryGetValue(milestoneName, out gitHubMilestone))
                {
                    var milestoneUpdate = new MilestoneUpdate
                    {
                        Title = gitHubMilestone.Title,
                        Description = codePlexRelease.Description,
                        State =
                            codePlexRelease.DevelopmentStatus == DevelopmentStatus.Planning
                                ? ItemState.Open
                                : ItemState.Closed,
                        DueOn = codePlexRelease.ReleaseDate
                    };

                    if (milestoneUpdate.Description != gitHubMilestone.Description ||
                        milestoneUpdate.State != gitHubMilestone.State || milestoneUpdate.DueOn != gitHubMilestone.DueOn)
                    {
                        gitHubMilestone =
                            await
                                _client.Issue.Milestone.Update(_repoOwner, _repoName, gitHubMilestone.Number,
                                    milestoneUpdate);

                        _milestoneMap[gitHubMilestone.Title] = gitHubMilestone;
                    }
                }
                else
                {
                    gitHubMilestone =
                        await _client.Issue.Milestone.Create(_repoOwner, _repoName,
                            new NewMilestone(milestoneName)
                            {
                                Description = codePlexRelease.Description,
                                State = codePlexRelease.DevelopmentStatus == DevelopmentStatus.Planning
                                    ? ItemState.Open
                                    : ItemState.Closed,
                                DueOn = codePlexRelease.ReleaseDate
                            });
                    _milestoneMap[gitHubMilestone.Title] = gitHubMilestone;
                }
            }

            // TODO: should we also delete unused existing milestones?
        }

        public async Task MigrateRepoLabelsAsync()
        {
            var current = (await _client.Issue.Labels.GetAllForRepository(_repoOwner, _repoName))
                .ToDictionary(l => l.Name, l => l.Color);

            var desired = new Dictionary<string, string>
            {
                {"bug", "fc2929"},
                {"duplicate", "cccccc"},
                {"enhancement", "84b6eb"},
                {"help wanted", "159818"},
                {"invalid", "e6e6e6"},
                {"question", "cc317c"},
                {"wontfix", "ffffff"},
                {"investigation-needed", "0052cc"},
                {"task", "d4c5f9"},
                {"perf", "fef2c0"},
                {"test", "c7def8"},
                {"up-for-grabs", "0090FF"},
                {"pri0", "e11d21"},
                {"pri1", "eb6420"},
                {"needs-review", "e11d21"},
                {"providers-beware", "fbca04"},
                {"cleanup", "bfdadc"},
                {"blocked", "e11d21"},
                {"working", "cccccc"},
                {"low-impact", "fad8c7"},
                {"high-impact", "f7c6c7"},
                {"+200votes", "5319e7"},
                {"+100votes", "5319e7"},
                {"+50votes", "5319e7"},
                {"+20votes", "5319e7"},
                {"+10votes", "5319e7"},
                {"regression", "e11d21"},
                {"fixed", "006b75"},
                {"bydesign", "006b75"},
                {"norepro", "006b75"},
                {"external", "006b75"},
                {"codeplex", "5ea8de"}
            };

            foreach (var component in _context.GetWorkItemComponents())
            {
                desired[GetGitHubName(component)] = "c7def8";
            }

            foreach (var label in desired)
            {
                var name = label.Key;
                var desiredColor = label.Value;
                string currentColor = null;
                if (!current.TryGetValue(name, out currentColor))
                {
                    await _client.Issue.Labels.Create(_repoOwner, _repoName, new NewLabel(name, desiredColor));
                }
                else if (currentColor != desiredColor)
                {
                    await _client.Issue.Labels.Update(_repoOwner, _repoName, name, new LabelUpdate(name, desiredColor));
                }
            }

            //foreach (var existingLabel in current.Except(desired))
            //{
            //    await _client.Issue.Labels.Delete(_repoOwner, _repoName, existingLabel.Key);
            //}
        }
    }
}