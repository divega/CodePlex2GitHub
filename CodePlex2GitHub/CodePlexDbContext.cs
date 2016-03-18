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


        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemClosingReason> WorkItemClosingReasons { get; set; }


        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<DiscussionTopic> DiscussionTopics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(new SqlConnection());

            optionsBuilder.UseInMemoryDatabase();
        }

        public IQueryable<WorkItem> GetWorkItemAggregates()
        {
            return this.WorkItems.OrderBy(i => i.Number)
                .Include(i => i.Comments).ThenInclude(c => c.PosteBy)
                .Include(i => i.AssignedTo)
                .Include(i => i.Attachments)
                .Include(i => i.ClosingReason)
                .Include(i => i.ClosedBy)
                .Include(i => i.Component)
                .Include(i => i.Release)
                .Include(i => i.ReportedBy)
                .Include(i => i.UpdatedBy);
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<WorkItem>(
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
