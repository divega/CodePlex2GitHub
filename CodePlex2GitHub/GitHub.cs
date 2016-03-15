using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        private Task<Octokit.Issue> CreateIssue()
        {
            return _client.Issue.Create(_repoOwner, _repoName, new NewIssue("Unused"));
        }
        private async Task<Octokit.IssueUpdate> GetOrAddIssue(int number)
        {
            var existingIssue = await _client.Issue.Get(_repoOwner, _repoName, number);
            if (existingIssue != null)
            {
                return existingIssue.ToUpdate();
            }
            var newIssue = await CreateIssue();
            var issueUpdate = newIssue.ToUpdate();
            while (newIssue.Number < number)
            {
                issueUpdate.State = ItemState.Closed;
                await UpdateIssue(newIssue.Number, issueUpdate);
                newIssue = await CreateIssue();
                issueUpdate = newIssue.ToUpdate();
            }

            if (newIssue.Number > number)
            {
                throw new InvalidOperationException($"Issues out of sequence: CodePlex #{number}, GitHub #{newIssue.Number}");
            }
            return issueUpdate;
        }

        private Task UpdateIssue(int number, IssueUpdate issueUpdate)
        {
            return _client.Issue.Update(_repoOwner, _repoName, number, issueUpdate);
        }

        public async Task CreateIssuesAsync()
        {
            // TODO: ForEachAsync has issue
            foreach (var codePlexIssue in _context.Issues.OrderBy(i => i.Number))
            {
                await CreateIssueAsync(codePlexIssue);
            }
        }

        public async Task CreateIssueAsync(Model.Issue codePlexIssue)
        {
            try
            {
                var update = await GetOrAddIssue(codePlexIssue.Number);
                update.Title = codePlexIssue.Title;
                update.Body = codePlexIssue.Body;
                update.Assignee = codePlexIssue.AssignedTo?.GitHubAlias;
                await UpdateIssue(codePlexIssue.Number, update);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }
    }

    public static class QueryExtensions
    {
        public async static Task MyForEachAsync<T>(this IQueryable<T> source, Action<T> action)
        {
            var enumerable = source.ToAsyncEnumerable();
            using (var enumerator = enumerable.GetEnumerator())
            {
                while (await enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }
    }
}

