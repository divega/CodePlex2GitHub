using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;
using Octokit;

namespace CodePlex2GitHub
{
    public class FakeData
    {
        public static void Add(CodePlexDbContext context)
        {
            Issues.Add(context);
            // TODO: make this async
            context.SaveChanges();
        }

        public static class Issues
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    new WorkItem
                    {
                        WorkItemId = 2,

                        Summary = "Investigate FxCop warning on disposable classes",
                        Description =
@"Hey @ajcvickers Currently there are quite a few cases where FxCop warnings have been suppressed for disposable classes. These suppressions may be okay, or the FxCop warnings may indicate real issues. Each one should be investigated.

Suppressions include CA1001, CA2213, CA1063.",
                        WorkItemComponent = SeedData.Components.Runtime,
                        ReasonClosed = SeedData.WorkItemClosingReasons.Fixed,
                        ReportedBy = SeedData.People.Arthur,
                        ReportedOn = new DateTime(2012, 4, 11),
                        UpdatedBy = SeedData.People.Murat,
                        UpatedOn = new DateTime(2014, 4, 25),
                        Release = SeedData.Releases.EF610,
                        Severity = WorkItem.WorkItemSeverity.Medium,
                        Status = WorkItem.WorkItemStatus.Closed,
                        Type = WorkItem.WorkItemType.Task,
                        Votes = 500,
                        Comments = new List<WorkItemComment>
                        {
                                        new WorkItemComment
                                        {
                                            PosteBy = SeedData.People.Brice,
                                            PostedOn = new DateTime(2014,1,20),
                                            Body = @"Fixed in changeset c720d567035de480fc3fe592d91052fd30934313"
                                        },
                                        new WorkItemComment
                                        {
                                            PosteBy = SeedData.People.Brice,
                                            PostedOn = new DateTime(2014,1,20),
                                            Body = @"Fixed in changeset f089babd48431bfbdb84fbaf9bc89270b213043e"
                                        }
                        }
                    },
                    new WorkItem
                    {
                        WorkItemId = 3,

                        Summary = "If spatial types cannot be loaded we should tell it to the user instead throwing an exception without message.",
                        Description = 
@"If Microsoft.Sql.Types.dll assembly is not present DefaultSpatialServices will throw NotImplementedException without any error message. It is hard to figure out why the exception is thrown and how to fix it - vide: 
http://stackoverflow.com/questions/10117008/net-4-5-beta-dbgeography-notimplementedexception
Also the exception probably should not be NotImplementedException",
                        WorkItemComponent = SeedData.Components.Runtime,
                        Status = WorkItem.WorkItemStatus.Closed,
                        ReasonClosed = SeedData.WorkItemClosingReasons.Fixed,
                        ReportedBy = SeedData.People.Pawel,
                        ReportedOn = new DateTime(2012, 4, 12),
                        UpdatedBy = SeedData.People.Eilon,
                        UpatedOn = new DateTime(2015, 7, 27),
                        Release = SeedData.Releases.EF600,
                        Severity = WorkItem.WorkItemSeverity.Medium,
                        Type = WorkItem.WorkItemType.Issue,
                        ClosedBy = SeedData.People.Luke,
                        ClosedOn = new DateTime(2013, 4, 12),
                        AssignedTo = SeedData.People.Arthur,
                        Votes = 275,
                        Comments = new List<WorkItemComment>
                        {
                                        new WorkItemComment
                                        {
                                            PosteBy = SeedData.People.Arthur,
                                            PostedOn = new DateTime(2013,3,20),
                                            Body = 
@"Fixed in b3eca2c141c0

SpacedOutAndThrowing (CodePlex 3: Provide more help when spatial provider fails to load than the current empty exception message)

Currently we throw an exception with no message when the default spatial provider is used and anything non-trivial is attempted. We should consider replacing/removing the default provider, but that is beyond the scope of this bug which is to provide more helpful guidance."
                                        },

                        }
                    });
            }
        }
    }
}
