using System.Data.SqlClient;
using System.Linq;
using CodePlex2GitHub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodePlex2GitHub
{
    public class CodePlexDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly int _projectId;

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

        public DbSet<User> User { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Thread> Thread { get; set; }
        public DbSet<WorkItemComment> WorkItemComments { get; set; }
        public DbSet<WorkItemAttachment> WorkItemAttachments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public IQueryable<WorkItem> GetWorkItems()
        {
            return ApplyTenantFilter(WorkItems)
                .Include(i => i.AssignedTo)
                .Include(i => i.ClosedBy)
                .Include(i => i.ReportedBy)
                .Include(i => i.LastUpdatedBy)
                .OrderBy(i => i.WorkItemId);
        }

        public IQueryable<WorkItemComment> GetWorkItemComments()
        {
            return ApplyTenantFilter(WorkItemComments.Include(c => c.User))
                ;
        } 

        public IQueryable<WorkItemAttachment> GetWorkItemAttachments()
        {
            return ApplyTenantFilter(WorkItemAttachments)
                .Include(a => a.FileAttachment.Content);
        }

        public IQueryable<Thread> GetThreadAggregates()
        {
            return ApplyTenantFilter(Set<Thread>()
                .FromSql(
                    @"select Thread.ThreadId, Thread.ProjectId, Title, TagName " +
                    @"from Thread " +
                    @"inner join ThreadTagAssociation ON Thread.ThreadId = ThreadTagAssociation.ThreadId " +
                    @"inner join ThreadTag ON ThreadTagAssociation.ThreadTagId = ThreadTag.ThreadTagId AND Thread.ProjectId = ThreadTag.ProjectId ")
                .Include(t => t.Posts));
        }


        public IQueryable<Release> GetReleases()
        {
            return ApplyTenantFilter(Set<Release>());
        }

        public IQueryable<string> GetWorkItemComponents()
        {
            return ApplyTenantFilter(WorkItems)
                .Select(i => i.Component)
                .Distinct()
                .Where(c => !string.IsNullOrEmpty(c));
        }

        private IQueryable<T> ApplyTenantFilter<T>(IQueryable<T> source) where T : class
            // https://github.com/aspnet/EntityFramework/issues/4875
            => source.Where(e => EF.Property<int>(e, "ProjectID") == _projectId);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Thread>(AddTenantId)
                .Entity<WorkItem>(AddTenantId)
                .Entity<WorkItemComment>(AddTenantId)
                .Entity<WorkItemAttachment>(AddTenantId)
                .Entity<Release>(AddTenantId);

            modelBuilder.Entity<WorkItem>().HasMany(i => i.Comments)
                .WithOne()
                .HasForeignKey(c => c.WorkItemId);

            modelBuilder.Entity<WorkItem>().HasMany(i => i.Attachments)
                .WithOne().HasForeignKey(a => a.WorkItemId);

            modelBuilder.Entity<FileAttachment>()
                .HasOne(f => f.Content)
                .WithOne()
                .HasForeignKey<FileAttachmentContent>(c => c.FileAttachmentId);

            modelBuilder.Entity<WorkItemAttachment>()
                .HasKey(nameof(WorkItemAttachment.WorkItemId), "FileAttachmentId");
            

            // Workaround: could be [ForeignKey("UserId")] but for https://github.com/aspnet/EntityFramework/issues/4909
            // Could even go away but for https://github.com/aspnet/EntityFramework/issues/4884
            modelBuilder.Entity<WorkItemComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey("UserId");
        }

        private void AddTenantId<T>(EntityTypeBuilder<T> builder) where T : class
        {
            builder.Property<int>("ProjectID");
        }
    }
}