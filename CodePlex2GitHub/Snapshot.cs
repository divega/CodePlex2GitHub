using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodePlex2GitHub;

namespace CodePlex2GitHub
{
    [DbContext(typeof(CodePlexDbContext))]
    partial class MsdodelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20143");

            modelBuilder.Entity("CodePlex2GitHub.Model.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Discussion", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<int?>("TopicId");

                    b.HasKey("Number");

                    b.HasIndex("TopicId");

                    b.ToTable("Discussion");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.DiscussionTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DiscussionTopic");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItem", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignedToAlias");

                    b.Property<string>("ClosedByAlias");

                    b.Property<DateTime>("ClosedOn");

                    b.Property<int?>("ClosingReasonId");

                    b.Property<int?>("ComponentId");

                    b.Property<int?>("Impact");

                    b.Property<int?>("ReleaseId");

                    b.Property<string>("ReportedByAlias");

                    b.Property<DateTime>("ReportedOn");

                    b.Property<int>("Status");

                    b.Property<string>("Body");

                    b.Property<string>("Title");

                    b.Property<int?>("Type");

                    b.Property<DateTime>("UpatedOn");

                    b.Property<string>("UpdatedByAlias");

                    b.Property<int>("Votes");

                    b.HasKey("Number");

                    b.HasIndex("AssignedToAlias");

                    b.HasIndex("ClosedByAlias");

                    b.HasIndex("ClosingReasonId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("ReleaseId");

                    b.HasIndex("ReportedByAlias");

                    b.HasIndex("UpdatedByAlias");

                    b.ToTable("WorkItem");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<int?>("IssueNumber");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("IssueNumber");

                    b.ToTable("WorkItemAttachment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemClosingReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("WorkItemClosingReason");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("IssueNumber");

                    b.Property<string>("PosteByPersonAlias");

                    b.Property<DateTime>("PostedOn");

                    b.Property<string>("Body");

                    b.HasKey("Id");

                    b.HasIndex("IssueNumber");

                    b.HasIndex("PosteByPersonAlias");

                    b.ToTable("WorkItemComment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Person", b =>
                {
                    b.Property<string>("Name");

                    b.Property<string>("GitHubName");

                    b.Property<bool>("IsTeamMember");

                    b.HasKey("Name");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<bool>("IsReleased");

                    b.HasKey("Id");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Discussion", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.DiscussionTopic")
                        .WithMany()
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItem", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("AssignedToAlias");

                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("ClosedByAlias");

                    b.HasOne("CodePlex2GitHub.Model.WorkItemClosingReason")
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
                        .HasForeignKey("ReportedByAlias");

                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("UpdatedByAlias");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("IssueNumber");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("IssueNumber");

                    b.HasOne("CodePlex2GitHub.Model.Person")
                        .WithMany()
                        .HasForeignKey("PosteByPersonAlias");
                });
        }
    }
}
