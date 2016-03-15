using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodePlex2GitHub;

namespace CodePlex2GitHub
{
    [DbContext(typeof(CodePlexDbContext))]
    partial class Dump : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20143")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodePlex2GitHub.Model.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Discussion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<int?>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Discussions");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.DiscussionTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("DiscussionTopics");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AssignedToPersonId");

                    b.Property<int?>("ClosingReasonId");

                    b.Property<int?>("ComponentId");

                    b.Property<int?>("Impact");

                    b.Property<int?>("ReleaseId");

                    b.Property<int>("ReportedByPersonId");

                    b.Property<DateTime>("ReportedOn");

                    b.Property<int>("Status");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<int?>("Type");

                    b.Property<DateTime>("UpatedOn");

                    b.Property<int?>("UpdatedByPersonId");

                    b.Property<int>("Votes");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToPersonId");

                    b.HasIndex("ClosingReasonId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("ReleaseId");

                    b.HasIndex("UpdatedByPersonId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.IssueAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<int?>("IssueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.ToTable("IssueAttachment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.IssueClosingReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("IssueClosingReasons");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.IssueComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("IssueId");

                    b.Property<DateTime>("PostedOn");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.ToTable("IssueComment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("GitHubAlias");

                    b.Property<bool>("IsTeamMember");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsReleased");

                    b.HasKey("Id");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Discussion", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.DiscussionTopic")
                        .WithMany()
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Issue", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("AssignedToPersonId");

                    b.HasOne("CodePlex2GitHub.Model.IssueClosingReason")
                        .WithMany()
                        .HasForeignKey("ClosingReasonId");

                    b.HasOne("CodePlex2GitHub.Model.Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");

                    b.HasOne("CodePlex2GitHub.Model.Release")
                        .WithMany()
                        .HasForeignKey("ReleaseId");

                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("UpdatedByPersonId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.IssueAttachment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.Issue")
                        .WithMany()
                        .HasForeignKey("IssueId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.IssueComment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.Issue")
                        .WithMany()
                        .HasForeignKey("IssueId");
                });
        }
    }
}
