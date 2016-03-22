using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using CodePlex2GitHub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodePlex2GitHub
{
    public class CodePlexDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly int _projectId = 0;
        public CodePlexDbContext(string databaseServer, string databaseName, int projectId)
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = databaseServer,
                InitialCatalog = databaseName,
                IntegratedSecurity = true
            };
            _connectionString = csb.ToString();
            _projectId = projectId;
        }

        public DbSet<User> People { get; set; }
        public DbSet<Release> Release { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Thread> Thread { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public IQueryable<WorkItem> GetWorkItemAggregates()
        {
            return ApplyTenantFilter(WorkItems
                .OrderBy(i => i.WorkItemId)
                .Include(i => i.AssignedTo)
                .Include(i => i.ClosedBy)
                .Include(i => i.ReportedBy)
                .Include(i => i.LastUpdatedBy).Include(i => i.Comments).ThenInclude(c => c.PosteBy)
                .Include(i => i.Attachments).ThenInclude(a => a.File.Content));
        }

        public IQueryable<Thread> GetThreadAggregates()
        {
            return ApplyTenantFilter(this.Set<Thread>()
                .FromSql(
                    @"select Thread.ThreadId, Thread.ProjectId, Title, TagName " +
                    @"from Thread " +
                    @"inner join ThreadTagAssociation ON Thread.ThreadId = ThreadTagAssociation.ThreadId " +
                    @"inner join ThreadTag ON ThreadTagAssociation.ThreadTagId = ThreadTag.ThreadTagId AND Thread.ProjectId = ThreadTag.ProjectId ")
                .Include(t => t.Posts));
        }


        public IQueryable<Release> GetReleases()
        {
            return ApplyTenantFilter(this.Set<Release>()); 
        }

        public IQueryable<string> GetWorkItemComponents()
        {
            return ApplyTenantFilter(this.WorkItems).Select(i => i.Component).Distinct().Where(c => !string.IsNullOrEmpty(c));
        }
        private IQueryable<T> ApplyTenantFilter<T>(IQueryable<T> source) where T: class // https://github.com/aspnet/EntityFramework/issues/4875
            => source.Where(e => EF.Property<int>(e, "ProjectID") == _projectId);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .Entity<Thread>(AddTenantId)
                .Entity<WorkItem>(AddTenantId)
                .Entity<WorkItemAttachment>(AddTenantId)
                .Entity<Release>(AddTenantId);
        }

        private void AddTenantId<T>(EntityTypeBuilder<T> builder) where T : class
        {
            builder.Property<int>("ProjectID");
        }
    }
}