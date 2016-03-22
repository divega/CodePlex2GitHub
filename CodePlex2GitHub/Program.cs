using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Threading;

namespace CodePlex2GitHub
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncPump.Run(async delegate
            {
                await MainAsync(args);

            });
        }

        static async Task MainAsync(string[] args)
        {
            using (var context = new CodePlexDbContext(args[0], args[1], int.Parse(args[2])))
            {
                DumpModelSnapshot(context);
                var gitHub = new GitHub(args[3], args[4], args[5], context);
                await gitHub.MigrateRepoLabelsAsync();
                await gitHub.MigrateMilestonesAsync();
                await gitHub.MigrateAllIssuesAsync();
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
