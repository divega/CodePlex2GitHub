using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using System.IO;

namespace CodePlex2GitHub
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CodePlexDbContext())
            {
                // DumpModel(context);
                SeedData.Add(context);
                AddFakeData(context);
            }
        }

        private static void DumpModel(DbContext context)
        {
            var codeHelper = new CSharpHelper();
            var generator = new CSharpMigrationsGenerator(
                codeHelper,
                new CSharpMigrationOperationGenerator(codeHelper),
                new CSharpSnapshotGenerator(codeHelper));

            var modelSnapshot = generator.GenerateSnapshot(
                context.GetType().Namespace,
                context.GetType(),
                "Snapshot",
                context.Model);
            File.WriteAllText("..\\..\\Snapshot.cs", modelSnapshot);
        }



        static void AddFakeData(CodePlexDbContext context)
        {
            context.AddRange(
                new Issue
                {
                    Number = 2,

                    Title = "Investigate FxCop warning on disposable classes",
                    Text = @"Currently there are quite a few cases where FxCop warnings have been suppressed for disposable classes. These suppressions may be okay, or the FxCop warnings may indicate real issues. Each one should be investigated.

Suppressions include CA1001, CA2213, CA1063.",
                    Component = SeedData.Components.Runtime,
                    ClosingReason = SeedData.IssueClosingReasons.Fixed,
                    ReportedBy = SeedData.People.Arthur,
                    ReportedOn = new DateTime(2012, 4, 11),
                    UpdatedBy = SeedData.People.Murat,
                    UpatedOn = new DateTime(2014, 4, 25),
                    Release = SeedData.Releases.EF610,
                    Impact = Issue.IssueImpact.Medium,
                    Status = Issue.IssueStatus.Closed,
                    Type = Issue.IssueType.Task,
                    Comments = new List<IssueComment>
                    {
                        new IssueComment
                        {
                            PosteByPerson = SeedData.People.Brice,
                            PostedOn = new DateTime(2014,1,20),
                            Text = @"Fixed in changeset c720d567035de480fc3fe592d91052fd30934313"
                        },
                        new IssueComment
                        {
                            PosteByPerson = SeedData.People.Brice,
                            PostedOn = new DateTime(2014,1,20),
                            Text = @"Fixed in changeset f089babd48431bfbdb84fbaf9bc89270b213043e"
                        }
                    }
                },
                new Issue
                {
                    Number = 2,

                    Title = "If spatial types cannot be loaded we should tell it to the user instead throwing an exception without message.",
                    Text = @"If Microsoft.Sql.Types.dll assembly is not present DefaultSpatialServices will throw NotImplementedException without any error message. It is hard to figure out why the exception is thrown and how to fix it - vide: 
http://stackoverflow.com/questions/10117008/net-4-5-beta-dbgeography-notimplementedexception
Also the exception probably should not be NotImplementedException",
                    Component = SeedData.Components.Runtime,
                    Status = Issue.IssueStatus.Closed,
                    ClosingReason = SeedData.IssueClosingReasons.Fixed,
                    ReportedBy = SeedData.People.Pawel,
                    ReportedOn = new DateTime(2012, 4, 12),
                    UpdatedBy = SeedData.People.Eilon,
                    UpatedOn = new DateTime(2015, 7, 27),
                    Release = SeedData.Releases.EF600,
                    Impact = Issue.IssueImpact.Medium,
                    Type = Issue.IssueType.Issue,
                    ClosedBy = SeedData.People.Luke,
                    ClosedOn = new DateTime(2013, 4, 12),
                    Comments = new List<IssueComment>
                    {
                        new IssueComment
                        {
                            PosteByPerson = SeedData.People.Arthur,
                            PostedOn = new DateTime(2013,3,20),
                            Text = @"Fixed in b3eca2c141c0

 SpacedOutAndThrowing (CodePlex 3: Provide more help when spatial provider fails to load than the current empty exception message)

 Currently we throw an exception with no message when the default spatial provider is used and anything non-trivial is attempted. We should consider replacing/removing the default provider, but that is beyond the scope of this bug which is to provide more helpful guidance."
                        },

                    }
                });
        }
    }
}
