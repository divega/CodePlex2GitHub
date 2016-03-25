using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Threading;

namespace CodePlex2GitHub
{
    class Program
    {
        static void Main(string[] args)
        {
            /* regex test
            var regex = new Regex(@"http://entityframework\.codeplex\.com/SourceControl/changeset/([0-9a-fA-F]+)$");

            var input = @"http://entityframework.codeplex.com/SourceControl/changeset/4a2ea1a9eb1f";

            var match = regex.Match(input);
            if (match.Success)
            {
                Console.WriteLine(match.Groups[0]);
            }
            */


            using (var context = new CodePlexDbContext(args[0], args[1], int.Parse(args[2])))
            {
                //DumpModelSnapshot(context);

                var gitHub = new GitHub(args[3], args[4], args[5], context);

                AsyncPump.Run(async delegate
                {
                    await gitHub.MigrateRepoLabelsAsync();
                    await gitHub.MigrateMilestonesAsync();
                    // TODO: Migrate team members (so we can set assignee)
                    await gitHub.MigrateAllIssuesAsync();
                    // TODO: Migrate discussion threads
                });
            }
        }

        private static void DumpModelSnapshot(DbContext context)
        {
            var codeHelper = new CSharpHelper();
            var generator = new CSharpMigrationsGenerator(
                codeHelper,
                new CSharpMigrationOperationGenerator(codeHelper),
                new CSharpSnapshotGenerator(codeHelper));

            var contextType = context.GetType();
            var contextTypeNamespace = contextType.Namespace;
            Debug.Assert(contextTypeNamespace != null);

            var modelSnapshot = generator.GenerateSnapshot(
                contextTypeNamespace,
                contextType,
                $"{contextType.Name}ModelSnapshot",
                context.Model);

            File.WriteAllText("..\\..\\ModelSnapshot.cs", modelSnapshot);
        }
    }
}
