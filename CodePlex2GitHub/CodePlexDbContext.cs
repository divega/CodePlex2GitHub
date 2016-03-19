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
            var csb = new SqlConnectionStringBuilder();
            csb.DataSource= databaseServer;
            csb.InitialCatalog = databaseName;
            _connectionString = csb.ToString(); 
            _projectId = projectId;
            }

        public DbSet<Person> People { get; set; }
        public DbSet<WorkItemComponent> WorkItemComponents { get; set; }
        public DbSet<Release> Release { get; set; }


        public DbSet<WorkItem> WorkItems { get; set; }



        public DbSet<Thread> Thread { get; set; }
        public DbSet<ThreadTag> ThreadTag { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);

            //optionsBuilder.UseInMemoryDatabase();
        }

        public IQueryable<WorkItem> GetWorkItemAggregates()
        {
            return WorkItems
                .Where(i => EF.Property<int>(i, "ProjectID") == _projectId)
                .OrderBy(i => i.WorkItemId)
                .Include(i => i.Comments).ThenInclude(c => c.PosteBy)
                .Include(i => i.AssignedTo)
                .Include(i => i.Attachments)
                .Include(i => i.ReasonClosed)
                .Include(i => i.ClosedBy)
                .Include(i => i.WorkItemComponent)
                .Include(i => i.Release)
                .Include(i => i.ReportedBy)
                .Include(i => i.UpdatedBy);
        }

        public IQueryable<Thread> GetThreadAggregates()
        {
            return ApplyTenantFilter(this.Set<Thread>()
                .FromSql(
                    @"SELECT Thread.ThreadId, Title, ThreadTagId " +
                    @"FROM Thread INNER JOIN ThreadTagAssociation ON Thread.ThreadId = ThreadTagAssociation.ThreadId")
                .Include(t => t.Tag)
                .Include(t => t.Posts));
        }

        public IQueryable<ThreadTag> GetThreadTags()
        {
            return ApplyTenantFilter(this.Set<ThreadTag>()
                .FromSql(
                    @"SELECT [ThreadTagId], [TagName], GitHubName = CASE TagName WHEN 'General' THEN null WHEN 'EF Runtime' THEN 'runtime' WHEN 'EF Power Tools' THEN 'powertools' WHEN 'EF Designer' THEN 'designer' END " +
                    @"FROM [Codeplex].[dbo].[ThreadTag]"));
        } 


        public IQueryable<Release> GetReleases()
        {
            return ApplyTenantFilter(this.Set<Release>()
                .FromSql(
                    @"select Id, Name, Description, ReleaseDate, IsReleased = CASE WHEN DevelopmentStatusId = 1 THEN 0 ELSE 1 END, IsInvestigation = CASE WHEN Name = 'Investigation' THEN 1 ELSE 0 END, GitHubName = CASE Name WHEN 'Future' THEN 'Backlog' WHEN 'Investigation' THEN 'Backlog' ELSE Name END " +
                    @"from release"));
        }

        public IQueryable<WorkItemComponent> GetWorkItemComponents()
        {
            return ApplyTenantFilter(this.Set<WorkItemComponent>()
                .FromSql(
                    @"SELECT [Component], GitHubName = CASE Component WHEN 'Build/Infra.' THEN 'build' WHEN 'Power Tools' THEN 'powertools' WHEN 'Tests' THEN 'test' ELSE lower(Component) END " +
                    @"FROM [Codeplex].[dbo].[WorkItemComponents]"
                ));
        } 

        private IQueryable<T> ApplyTenantFilter<T>(IQueryable<T> source) => source.Where(e => EF.Property<int>(e, "ProjectID") == _projectId);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
                .Entity<Thread>(AddTenantId)
                .Entity<ThreadTag>(AddTenantId)
                .Entity<WorkItem>(AddTenantId)
                .Entity<Thread>(AddTenantId)
                .Entity<WorkItemComponent>(AddTenantId);
        }

        private void AddTenantId<T>(EntityTypeBuilder<T> builder) where T: class 
        {
           builder.Property<int>("ProjectID");
        }
    }
}