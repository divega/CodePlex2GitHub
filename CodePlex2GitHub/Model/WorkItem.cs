using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace CodePlex2GitHub.Model
{
    public class WorkItem
    {
        [Key]
        public int WorkItemId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Person ReportedBy { get; set; }
        public DateTime ReportedOn { get; set; }
        public Person UpdatedBy { get; set; }
        public DateTime UpatedOn { get; set; }
        public Person AssignedTo { get; set; }
        public Person ClosedBy { get; set; }
        public DateTime ClosedOn { get; set; }
        public int Votes { get; set; } = 1;
        public WorkItemStatus Status { get; set; }
        public WorkItemReasonClosed ReasonClosed { get; set; }
        public WorkItemSeverity? Severity { get; set; }
        public WorkItemType? Type { get; set; }
        public Release Release { get; set; }
        public WorkItemComponent WorkItemComponent { get; set; }
        public ICollection<WorkItemAttachment> Attachments { get; set; }
        public ICollection<WorkItemComment> Comments { get; set; }
        public enum WorkItemStatus
        {
            Proposed,
            Active,
            Resolved,
            Closed
        }

        public enum WorkItemSeverity
        {
            Low = 50,
            Medium = 100,
            High = 150
        }

        public enum WorkItemType
        {
            Task,
            Issue,
            Feature
        }
    }
}
