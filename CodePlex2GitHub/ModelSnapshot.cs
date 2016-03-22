using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodePlex2GitHub;

namespace CodePlex2GitHub
{
    [DbContext(typeof(CodePlexDbContext))]
    partial class CodePlexDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20143")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodePlex2GitHub.Model.FileAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContentFileAttachmentId");

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.HasIndex("ContentFileAttachmentId");

                    b.ToTable("FileAttachment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.FileAttachmentContent", b =>
                {
                    b.Property<int>("FileAttachmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.HasKey("FileAttachmentId");

                    b.ToTable("FileAttachmentContent");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("DevelopmentStatus")
                        .HasColumnName("DevelopmentStatusId");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectID");

                    b.Property<DateTime?>("ReleaseDate");

                    b.HasKey("Id");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.Thread", b =>
                {
                    b.Property<int>("ThreadId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectID");

                    b.Property<string>("Tag");

                    b.Property<string>("Title");

                    b.HasKey("ThreadId");

                    b.ToTable("Thread");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.ThreadPost", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MarkedAsAnswerBy");

                    b.Property<DateTime?>("MarkedAsAnswerDate");

                    b.Property<string>("PostedBy");

                    b.Property<DateTime>("PostedDate");

                    b.Property<string>("Text");

                    b.Property<int?>("ThreadThreadId");

                    b.HasKey("PostId");

                    b.HasIndex("ThreadThreadId");

                    b.ToTable("ThreadPost");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.User", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("Name");

                    b.HasKey("UserId");

                    b.HasIndex("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItem", b =>
                {
                    b.Property<int>("WorkItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignedToUserID");

                    b.Property<int>("ClosedByUserID");

                    b.Property<DateTime>("ClosedDate");

                    b.Property<string>("Component");

                    b.Property<string>("Description");

                    b.Property<int>("LastUpdatedByUserID");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("PlannedForRelease");

                    b.Property<int>("ProjectID");

                    b.Property<string>("ReasonClosed");

                    b.Property<int>("ReportedByUserID");

                    b.Property<DateTime>("ReportedDate");

                    b.Property<int>("Severity");

                    b.Property<string>("Status");

                    b.Property<string>("Summary");

                    b.Property<string>("Type");

                    b.Property<int>("VoteCount");

                    b.HasKey("WorkItemId");

                    b.HasIndex("AssignedToUserID");

                    b.HasIndex("ClosedByUserID");

                    b.HasIndex("LastUpdatedByUserID");

                    b.HasIndex("ReportedByUserID");

                    b.ToTable("WorkItems");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.Property<int>("WorkItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FileAttachmentId");

                    b.Property<int>("ProjectID");

                    b.Property<int?>("WorkItemWorkItemId");

                    b.HasKey("WorkItemId");

                    b.HasIndex("FileAttachmentId");

                    b.HasIndex("WorkItemWorkItemId");

                    b.ToTable("WorkItemAttachments");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectID");

                    b.Property<int?>("WorkItemWorkItemId");

                    b.HasKey("CommentId");

                    b.HasIndex("WorkItemWorkItemId");

                    b.ToTable("WorkItemComments");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.FileAttachment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.FileAttachmentContent")
                        .WithMany()
                        .HasForeignKey("ContentFileAttachmentId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.ThreadPost", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.Thread")
                        .WithMany()
                        .HasForeignKey("ThreadThreadId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.User", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.WorkItemComment")
                        .WithOne()
                        .HasForeignKey("CodePlex2GitHub.Model.User", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItem", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("AssignedToUserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("ClosedByUserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("LastUpdatedByUserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("ReportedByUserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.FileAttachment")
                        .WithMany()
                        .HasForeignKey("FileAttachmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("WorkItemWorkItemId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("WorkItemWorkItemId");
                });
        }
    }
}
