using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Octokit;

namespace CodePlex2GitHub.Model
{
    public class WorkItem
    {
        [Key]
        public int WorkItemId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public User ReportedBy { get; set; }
        public DateTime ReportedDate { get; set; }
        [ForeignKey("LastUpdatedByUserID")]
        public User LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        [ForeignKey("AssignedToUserID")]
        public User AssignedTo { get; set; }
        [ForeignKey("ClosedByUserID")]
        public User ClosedBy { get; set; }
        public DateTime ClosedDate { get; set; }
        public int VoteCount { get; set; } = 1;
        public string Status { get; set; }
        public string ReasonClosed { get; set; }
        public WorkItemSeverity Severity { get; set; }
        public string Type { get; set; }
        public string PlannedForRelease { get; set; }
        public string Component { get; set; }
        public ICollection<WorkItemAttachment> Attachments { get; set; }
        public ICollection<WorkItemComment> Comments { get; set; }
        public static class WorkItemStatus
        {
            public static string Proposed => nameof(Proposed);
            public static  string Active => nameof(Active);
            public static string Resolved => nameof(Resolved);
            public static string Closed => nameof(Closed);
        }

        public enum WorkItemSeverity
        {
            Unassigned = 0,
            Low = 50,
            Medium = 100,
            High = 150
        }

        public static class WorkItemType
        {
            public static string Unassigned => nameof(Unassigned);
            public static string Task => nameof(Feature);
            public static string Issue => nameof(Feature);
            public static string Feature => nameof(Feature);
        }
    }
}
