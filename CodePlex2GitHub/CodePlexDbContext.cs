using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodePlex2GitHub.Model;

namespace CodePlex2GitHub
{
    public class CodePlexDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Release> Releases { get; set; }


        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueClosingReason> IssueClosingReasons { get; set; }


        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<DiscussionTopic> DiscussionTopics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(new SqlConnection());

            optionsBuilder.UseInMemoryDatabase();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Issue>(
                    config =>
                    {
                        config.HasKey(issue => issue.Number);
                    })
                .Entity<Discussion>(
                    config =>
                    {
                        config.HasKey(discussion => discussion.Number);
                    })
                .Entity<Person>(
                    config =>
                    {
                        config.HasKey(person => person.Alias);
                    });
        }
    }
}
