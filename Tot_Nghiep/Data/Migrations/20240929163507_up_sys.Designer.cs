﻿// <auto-generated />
using System;
using Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240929163507_up_sys")]
    partial class up_sys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Data.Model.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("GradeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxStudent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("TeacherId");

                    b.ToTable("classes");
                });

            modelBuilder.Entity("Data.Model.Exam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("exams");
                });

            modelBuilder.Entity("Data.Model.Exam_Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ExamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TeacherId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherId2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.HasIndex("RoomId");

                    b.HasIndex("TeacherId1");

                    b.HasIndex("TeacherId2");

                    b.ToTable("exam_Rooms");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CheckinImage")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("ChenkTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ExamRoomTestCodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Exam_Room_TestCodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Exam_Room_TestCodeId");

                    b.HasIndex("StudentId");

                    b.ToTable("exam_Room_Students");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_Student_AnswerHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ExamRoomStudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Exam_Room_StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TestQuestionAnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Exam_Room_StudentId");

                    b.HasIndex("TestQuestionAnswerId");

                    b.ToTable("exam_Room_Student_AnswerHistories");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_TestCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ExamRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Exam_RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TestCodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Exam_RoomId");

                    b.HasIndex("TestCodeId");

                    b.ToTable("exam_Room_TestCodes");
                });

            modelBuilder.Entity("Data.Model.ExamHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ExamRoomStudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Exam_Room_StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Exam_Room_StudentId");

                    b.ToTable("examHistories");
                });

            modelBuilder.Entity("Data.Model.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("grades");
                });

            modelBuilder.Entity("Data.Model.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("notifications");
                });

            modelBuilder.Entity("Data.Model.Notification_Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("NotificationId");

                    b.ToTable("notification_Classes");
                });

            modelBuilder.Entity("Data.Model.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Data.Model.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("rooms");
                });

            modelBuilder.Entity("Data.Model.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("students");
                });

            modelBuilder.Entity("Data.Model.Student_Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("JoinTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StudentProfilePhoto")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("student_Classes");
                });

            modelBuilder.Entity("Data.Model.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("subjects");
                });

            modelBuilder.Entity("Data.Model.Subject_Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GradeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("SubjectId");

                    b.ToTable("subjects_Grades");
                });

            modelBuilder.Entity("Data.Model.SystemConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsViewed")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("systemConfigs");
                });

            modelBuilder.Entity("Data.Model.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("teachers");
                });

            modelBuilder.Entity("Data.Model.Teacher_Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("teacher_Subjects");
                });

            modelBuilder.Entity("Data.Model.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("tests");
                });

            modelBuilder.Entity("Data.Model.TestCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("testCodes");
                });

            modelBuilder.Entity("Data.Model.TestQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RightAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TestCodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestCodeId");

                    b.ToTable("testQuestions");
                });

            modelBuilder.Entity("Data.Model.TestQuestionAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TestQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TestQuestionId");

                    b.ToTable("testQuestionAnswers");
                });

            modelBuilder.Entity("Data.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avartar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastMordificationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LockedEndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Data.Model.Class", b =>
                {
                    b.HasOne("Data.Model.Grade", "Grade")
                        .WithMany("Class")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Teacher", "Teacher")
                        .WithMany("Class")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Data.Model.Exam", b =>
                {
                    b.HasOne("Data.Model.Subject", "Subject")
                        .WithMany("Exam")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Data.Model.Exam_Room", b =>
                {
                    b.HasOne("Data.Model.Exam", "Exam")
                        .WithMany("Exam_Room")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Room", "Room")
                        .WithMany("Exam_Room")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Teacher", "Teacher1")
                        .WithMany("Exam_RoomsAsTeacher1")
                        .HasForeignKey("TeacherId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Teacher", "Teacher2")
                        .WithMany("Exam_RoomsAsTeacher2")
                        .HasForeignKey("TeacherId2")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");

                    b.Navigation("Room");

                    b.Navigation("Teacher1");

                    b.Navigation("Teacher2");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_Student", b =>
                {
                    b.HasOne("Data.Model.Exam_Room_TestCode", "Exam_Room_TestCode")
                        .WithMany("Exam_Room_Students")
                        .HasForeignKey("Exam_Room_TestCodeId");

                    b.HasOne("Data.Model.Student", "Student")
                        .WithMany("Exam_Room_Student")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam_Room_TestCode");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_Student_AnswerHistory", b =>
                {
                    b.HasOne("Data.Model.Exam_Room_Student", "Exam_Room_Student")
                        .WithMany("Exam_Room_Student_AnswerHistory")
                        .HasForeignKey("Exam_Room_StudentId");

                    b.HasOne("Data.Model.TestQuestionAnswer", "TestQuestionAnswer")
                        .WithMany("Exam_Room_Student_AnswerHistories")
                        .HasForeignKey("TestQuestionAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam_Room_Student");

                    b.Navigation("TestQuestionAnswer");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_TestCode", b =>
                {
                    b.HasOne("Data.Model.Exam_Room", "Exam_Room")
                        .WithMany("Exam_Room_TestCode")
                        .HasForeignKey("Exam_RoomId");

                    b.HasOne("Data.Model.TestCode", "TestCode")
                        .WithMany("Exam_Room_TestCodes")
                        .HasForeignKey("TestCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam_Room");

                    b.Navigation("TestCode");
                });

            modelBuilder.Entity("Data.Model.ExamHistory", b =>
                {
                    b.HasOne("Data.Model.Exam_Room_Student", "Exam_Room_Student")
                        .WithMany("ExamHistory")
                        .HasForeignKey("Exam_Room_StudentId");

                    b.Navigation("Exam_Room_Student");
                });

            modelBuilder.Entity("Data.Model.Notification_Class", b =>
                {
                    b.HasOne("Data.Model.Class", "Class")
                        .WithMany("Notification_Classe")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Notification", "Notification")
                        .WithMany("Notification_Classe")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("Data.Model.Student", b =>
                {
                    b.HasOne("Data.Model.User", "User")
                        .WithMany("Student")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Model.Student_Class", b =>
                {
                    b.HasOne("Data.Model.Class", "Class")
                        .WithMany("Student_Classes")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Student", "Student")
                        .WithMany("Student_Class")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Data.Model.Subject_Grade", b =>
                {
                    b.HasOne("Data.Model.Grade", "Grade")
                        .WithMany("Subject_Grades")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Subject", "Subject")
                        .WithMany("Subject_Grade")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Data.Model.Teacher", b =>
                {
                    b.HasOne("Data.Model.User", "User")
                        .WithMany("Teacher")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Model.Teacher_Subject", b =>
                {
                    b.HasOne("Data.Model.Subject", "Subject")
                        .WithMany("Teacher_Subject")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Model.Teacher", "Teacher")
                        .WithMany("Teacher_Subject")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Data.Model.Test", b =>
                {
                    b.HasOne("Data.Model.Subject", "Subject")
                        .WithMany("Test")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Data.Model.TestCode", b =>
                {
                    b.HasOne("Data.Model.Test", "Test")
                        .WithMany("TestCodes")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Data.Model.TestQuestion", b =>
                {
                    b.HasOne("Data.Model.TestCode", "TestCode")
                        .WithMany("TestQuestion")
                        .HasForeignKey("TestCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestCode");
                });

            modelBuilder.Entity("Data.Model.TestQuestionAnswer", b =>
                {
                    b.HasOne("Data.Model.TestQuestion", "TestQuestion")
                        .WithMany("TestQuestionAnswer")
                        .HasForeignKey("TestQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestQuestion");
                });

            modelBuilder.Entity("Data.Model.User", b =>
                {
                    b.HasOne("Data.Model.Role", "Role")
                        .WithMany("User")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Data.Model.Class", b =>
                {
                    b.Navigation("Notification_Classe");

                    b.Navigation("Student_Classes");
                });

            modelBuilder.Entity("Data.Model.Exam", b =>
                {
                    b.Navigation("Exam_Room");
                });

            modelBuilder.Entity("Data.Model.Exam_Room", b =>
                {
                    b.Navigation("Exam_Room_TestCode");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_Student", b =>
                {
                    b.Navigation("ExamHistory");

                    b.Navigation("Exam_Room_Student_AnswerHistory");
                });

            modelBuilder.Entity("Data.Model.Exam_Room_TestCode", b =>
                {
                    b.Navigation("Exam_Room_Students");
                });

            modelBuilder.Entity("Data.Model.Grade", b =>
                {
                    b.Navigation("Class");

                    b.Navigation("Subject_Grades");
                });

            modelBuilder.Entity("Data.Model.Notification", b =>
                {
                    b.Navigation("Notification_Classe");
                });

            modelBuilder.Entity("Data.Model.Role", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Data.Model.Room", b =>
                {
                    b.Navigation("Exam_Room");
                });

            modelBuilder.Entity("Data.Model.Student", b =>
                {
                    b.Navigation("Exam_Room_Student");

                    b.Navigation("Student_Class");
                });

            modelBuilder.Entity("Data.Model.Subject", b =>
                {
                    b.Navigation("Exam");

                    b.Navigation("Subject_Grade");

                    b.Navigation("Teacher_Subject");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Data.Model.Teacher", b =>
                {
                    b.Navigation("Class");

                    b.Navigation("Exam_RoomsAsTeacher1");

                    b.Navigation("Exam_RoomsAsTeacher2");

                    b.Navigation("Teacher_Subject");
                });

            modelBuilder.Entity("Data.Model.Test", b =>
                {
                    b.Navigation("TestCodes");
                });

            modelBuilder.Entity("Data.Model.TestCode", b =>
                {
                    b.Navigation("Exam_Room_TestCodes");

                    b.Navigation("TestQuestion");
                });

            modelBuilder.Entity("Data.Model.TestQuestion", b =>
                {
                    b.Navigation("TestQuestionAnswer");
                });

            modelBuilder.Entity("Data.Model.TestQuestionAnswer", b =>
                {
                    b.Navigation("Exam_Room_Student_AnswerHistories");
                });

            modelBuilder.Entity("Data.Model.User", b =>
                {
                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });
#pragma warning restore 612, 618
        }
    }
}
