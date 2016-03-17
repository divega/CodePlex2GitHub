using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodePlex2GitHub.Model;

namespace CodePlex2GitHub
{
    public static class SeedData
    {
        public static void Add(CodePlexDbContext context)
        {
            Components.Add(context);
            Releases.Add(context);
            DiscussionTopics.Add(context);
            WorkItemClosingReasons.Add(context);
            People.Add(context);
            // TODO: make this async
            context.SaveChanges();
        }

        public static class Components
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    BuildInfraestructure,
                    Designer,
                    Documentation,
                    Migrations,
                    PowerTools,
                    PowerShell,
                    Runtime,
                    Templates,
                    Tests);
            }

            public static Component BuildInfraestructure => new Component { Id = 1, Description = "Build/Infraesctructure" };
            public static Component Designer => new Component { Id = 2, Description = "Designer" };
            public static Component Documentation => new Component { Id = 3, Description = "Documentation" };
            public static Component Migrations => new Component { Id = 4, Description = "Migrations" };
            public static Component PowerTools => new Component { Id = 5, Description = "Power Tools" };
            public static Component PowerShell => new Component { Id = 6, Description = "PowerShell" };
            public static Component Runtime => new Component { Id = 7, Description = "Runtime" };
            public static Component Templates => new Component { Id = 8, Description = "Templates" };
            public static Component Tests => new Component { Id = 9, Description = "Tests" };
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static class Releases
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    EF500,
                    EF600,
                    EF601,
                    EF602,
                    EF610,
                    EF611,
                    EF612,
                    EF613,
                    EF620,
                    Future,
                    Investigation);
            }
            public static Release EF500 => new Release
            {
                Id = 1, Name = "EF 5.0.0", GitHubName = "EF 5.0.0", IsReleased = true, ReleaseDate = new DateTime(2012, 8, 11), Body =
                    @"The final release of Entity Framework 5 (EF5) is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release."
            };
            public static Release EF600 => new Release
            {
                Id = 2, Name = "EF 6.0.0", GitHubName = "EF 6.0.0", IsReleased = true, ReleaseDate = new DateTime(2013, 10, 17), Body =
                    @"The final release of Entity Framework 6 (EF6) is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release."
            };
            public static Release EF601 => new Release
            {
                Id = 3, Name = "EF 6.0.1", GitHubName = "EF 6.0.1", IsReleased = true, ReleaseDate = new DateTime(2013, 10, 17), Body =
                    @"The final release of the EF 6.0.1 patch is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release."
            };
            public static Release EF602 => new Release
            {
                Id = 4, Name = "EF 6.0.2", GitHubName = "EF 6.0.2", IsReleased = true, ReleaseDate = new DateTime(2013, 12, 13), Body =
                    @"The final release of the EF 6.0.2 patch is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release."
            };
            public static Release EF610 => new Release
            {
                Id = 5, Name = "EF 6.1.0", GitHubName = "EF 6.1.0", IsReleased = true, ReleaseDate = new DateTime(2014, 2, 11), Body=
                    @"The final release of the EF 6.1.0 release is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* 

See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release.

For details on how to get the latest runtime and tooling, see the [url:Get It page|http://msdn.microsoft.com/en-us/data/ee712906]."
            };
            public static Release EF611 => new Release
            {
                Id = 6, Name = "EF 6.1.1", GitHubName = "EF 6.1.1", IsReleased = true, ReleaseDate = new DateTime(2014, 6, 20), Body=
                    @"The final release of the EF 6.1.1 release is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* 

See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release.

For details on how to get the latest runtime and tooling, see the [url:Get It page|http://msdn.microsoft.com/en-us/data/ee712906]."
            };
            public static Release EF612 => new Release
            {
                Id = 7, Name = "EF 6.1.2", GitHubName = "EF 6.1.2", IsReleased = true, ReleaseDate = new DateTime(2014, 12, 22), Body =
                    @"The final release of the EF 6.1.2 release is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* 

See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release.

For details on how to get the latest runtime and tooling, see the [url:Get It page|http://msdn.microsoft.com/en-us/data/ee712906]."
            };
            public static Release EF613 => new Release { Id = 8, Name = "EF 6.1.3", GitHubName = "EF 6.1.3", IsReleased = true, ReleaseDate = new DateTime(2015, 3, 10), Body =
@"The final release of the EF 6.1.3 release is now available. 

*Visit [url:http://msdn.com/data/ef] to get started with this release.* 

See the [url:Entity Framework Version History|http://msdn.com/data/jj574253] for a list of new features in this release.

For details on how to get the latest runtime and tooling, see the [url:Get It page|http://msdn.microsoft.com/en-us/data/ee712906]."
            };
            public static Release EF620 => new Release { Id = 9, Name = "EF 6.2.0", GitHubName = "EF 6.2.0" };
            public static Release Future => new Release
            {
                Id = 1000, Name = "Future", GitHubName = "Backlog", Body =
                    @"This release is used to track work items that have been approved but are not scheduled for the current release."
            };
            public static Release Investigation => new Release
            {
                Id = 1001, Name = "Investigation", GitHubName = "Backlog", IsInvestigation = true, Body =
                    @"This release is used to track work items that need to be investigated to determine if they are legitimate issues and whether they should be fixed in the current release."
            };
        }

        public static class DiscussionTopics
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    Designer,
                    PowerTools,
                    Runtime,
                    General);
            }
            public static DiscussionTopic Designer => new DiscussionTopic { Id = 1, Description = "EF Designer" };
            public static DiscussionTopic PowerTools => new DiscussionTopic { Id = 2, Description = "EF PowerTools" };
            public static DiscussionTopic Runtime => new DiscussionTopic { Id = 3, Description = "EF Runtime" };
            public static DiscussionTopic General => new DiscussionTopic { Id = 4, Description = "General" };


        }

        public static class WorkItemClosingReasons
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    Fixed,
                    ByDesign,
                    CouldNotRepro,
                    Duplicate,
                    ExternalIssue,
                    WontFix);
            }
            public static WorkItemClosingReason Fixed => new WorkItemClosingReason { Id = 1, Description = "Fixed" };
            public static WorkItemClosingReason ByDesign => new WorkItemClosingReason { Id = 2, Description = "By Design" };
            public static WorkItemClosingReason CouldNotRepro => new WorkItemClosingReason { Id = 3, Description = "Could Not Repro" };
            public static WorkItemClosingReason Duplicate => new WorkItemClosingReason { Id = 4, Description = "Duplicate" };
            public static WorkItemClosingReason ExternalIssue => new WorkItemClosingReason { Id = 5, Description = "External Issue" };
            public static WorkItemClosingReason WontFix => new WorkItemClosingReason { Id = 6, Description = "Won't Fix" };
        }

        public static class People
        {
            public static void Add(CodePlexDbContext context)
            {
                context.AddRange(
                    Arthur,
                    Andrew,
                    Andriy,
                    Brennan,
                    Brice,
                    David,
                    Diego,
                    Eilon,
                    Emil,
                    Lawrence,
                    Maurycy,
                    Murat,
                    Rowan,
                    Smit,
                    Pawel);
            }
            public static Person Arthur
                => new Person { Alias = "ajcvickers", GitHubAlias = "ajcvickers", IsTeamMember = true };

            public static Person Andrew
                => new Person { Alias = "AndrewPeters", GitHubAlias = "anpete", IsTeamMember = true };

            public static Person Andriy
                => new Person { Alias = "AndriySvyryd", GitHubAlias = "AndriySvyryd", IsTeamMember = true };

            public static Person Brennan
                => new Person { Alias = "brecon", GitHubAlias = "BrennanConroy", IsTeamMember = true };

            public static Person Brice
                => new Person { Alias = "BriceLambson", GitHubAlias = "bricelam", IsTeamMember = true };

            public static Person David
                => new Person { Alias = "DavidObando", GitHubAlias = "DavidObando", IsTeamMember = true };

            public static Person Diego
                => new Person { Alias = "divega", GitHubAlias = "divega", IsTeamMember = true };

            public static Person Eilon
                => new Person { Alias = "eilonlipton", GitHubAlias = "eilon", IsTeamMember = true };

            public static Person Emil
                => new Person { Alias = "emilcicos", GitHubAlias = "emilcicos", IsTeamMember = true };

            public static Person Lawrence
                => new Person { Alias = "lajones", GitHubAlias = "lajones", IsTeamMember = true };

            public static Person Maurycy
                => new Person { Alias = "maumar", GitHubAlias = "maumar", IsTeamMember = true };

            public static Person Murat
                => new Person { Alias = "mgirgin", GitHubAlias = "muratg", IsTeamMember = true };

            public static Person Rowan
                => new Person { Alias = "RoMiller", GitHubAlias = "rowanmiller", IsTeamMember = true };

            public static Person Smit
                => new Person { Alias = "smitpatel", GitHubAlias = "smitpatel", IsTeamMember = true };

            public static Person Pawel
                => new Person { Alias = "moozzyk", GitHubAlias = "moozzyk", IsTeamMember = true };

            public static Person Luke
                => new Person { Alias = "lukew", GitHubAlias = "lukewaters", IsTeamMember = true };

        }
    }
}
