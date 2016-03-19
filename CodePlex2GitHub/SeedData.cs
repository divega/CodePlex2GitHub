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
            WorkItemClosingReasons.Add(context);
            People.Add(context);
            // TODO: make this async
            context.SaveChanges();
        }



        [SuppressMessage("ReSharper", "InconsistentNaming")]
       



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
            public static WorkItemReasonClosed Fixed => new WorkItemReasonClosed { Name = "Fixed", GitHubName = "fixed"};
            public static WorkItemReasonClosed ByDesign => new WorkItemReasonClosed { Name = "By Design", GitHubName = "bydesign"};
            public static WorkItemReasonClosed CouldNotRepro => new WorkItemReasonClosed { Name = "Could Not Repro", GitHubName = "norepro"};
            public static WorkItemReasonClosed Duplicate => new WorkItemReasonClosed { Name = "Duplicate", GitHubName = "duplicate" };
            public static WorkItemReasonClosed ExternalIssue => new WorkItemReasonClosed { Name = "External Issue", GitHubName = "external"};
            public static WorkItemReasonClosed WontFix => new WorkItemReasonClosed { Name = "Won't Fix", GitHubName = "wontfix"};
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
