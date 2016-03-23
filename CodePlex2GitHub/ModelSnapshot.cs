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

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.ToTable("FileAttachment");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.FileAttachmentContent", b =>
                {
                    b.Property<int>("FileAttachmentId");

                    b.Property<byte[]>("Content");

                    b.HasKey("FileAttachmentId");

                    b.HasIndex("FileAttachmentId");

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

                    b.Property<int?>("AssignedToUserId");

                    b.Property<int?>("ClosedByUserId");

                    b.Property<DateTime>("ClosedDate");

                    b.Property<string>("Component");

                    b.Property<string>("Description");

                    b.Property<int?>("LastUpdatedByUserId");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<string>("PlannedForRelease");

                    b.Property<int>("ProjectID");

                    b.Property<string>("ReasonClosed");

                    b.Property<int?>("ReportedByUserId");

                    b.Property<DateTime>("ReportedDate");

                    b.Property<int>("Severity");

                    b.Property<string>("Status");

                    b.Property<string>("Summary");

                    b.Property<string>("Type");

                    b.Property<int>("VoteCount");

                    b.HasKey("WorkItemId");

                    b.HasIndex("AssignedToUserId");

                    b.HasIndex("ClosedByUserId");

                    b.HasIndex("LastUpdatedByUserId");

                    b.HasIndex("ReportedByUserId");

                    b.ToTable("WorkItems");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.Property<int>("WorkItemId");

                    b.Property<int>("FileAttachmentId");

                    b.Property<int>("ProjectID");

                    b.HasKey("WorkItemId");

                    b.HasIndex("FileAttachmentId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("WorkItemAttachments");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectID");

                    b.Property<int>("WorkItemId");

                    b.HasKey("CommentId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("WorkItemComments");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.FileAttachmentContent", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.FileAttachment")
                        .WithOne()
                        .HasForeignKey("CodePlex2GitHub.Model.FileAttachmentContent", "FileAttachmentId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                        .HasForeignKey("AssignedToUserId");

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("ClosedByUserId");

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("LastUpdatedByUserId");

                    b.HasOne("CodePlex2GitHub.Model.User")
                        .WithMany()
                        .HasForeignKey("ReportedByUserId");
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemAttachment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.FileAttachment")
                        .WithMany()
                        .HasForeignKey("FileAttachmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodePlex2GitHub.Model.WorkItemComment", b =>
                {
                    b.HasOne("CodePlex2GitHub.Model.WorkItem")
                        .WithMany()
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
