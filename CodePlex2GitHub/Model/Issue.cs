using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace CodePlex2GitHub.Model
{
    public class Issue
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Person ReportedBy { get; set; }
        public DateTime ReportedOn { get; set; }
        public Person UpdatedBy { get; set; }
        public DateTime UpatedOn { get; set; }
        public Person AssignedTo { get; set; }
        public Person ClosedBy { get; set; }
        public DateTime ClosedOn { get; set; }
        public int Votes { get; set; } = 1;
        public IssueStatus Status { get; set; }
        public IssueClosingReason ClosingReason { get; set; }
        public IssueImpact? Impact { get; set; }
        public IssueType? Type { get; set; }
        public Release Release { get; set; }
        public Component Component { get; set; }
        public ICollection<IssueAttachment> Attachments { get; set; }
        public ICollection<IssueComment> Comments { get; set; }
        public enum IssueStatus
        {
            Proposed,
            Active,
            Resolved,
            Closed
        }

        public enum IssueImpact
        {
            Low,
            Medium,
            High
        }

        public enum IssueType
        {
            Task,
            Issue,
            Feature
        }
    }
}
