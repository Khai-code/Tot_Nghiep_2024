using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "systemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_systemConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Avartar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockedEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMordificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exams_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subjects_Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subjects_Grades_grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subjects_Grades_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tests_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_students_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teachers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "testCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_testCodes_tests_TestId",
                        column: x => x.TestId,
                        principalTable: "tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MaxStudent = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_classes_grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classes_teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_Rooms_exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_Rooms_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_Rooms_teachers_TeacherId1",
                        column: x => x.TeacherId1,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_exam_Rooms_teachers_TeacherId2",
                        column: x => x.TeacherId2,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "teacher_Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teacher_Subjects_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_Subjects_teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "testQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RightAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_testQuestions_testCodes_TestCodeId",
                        column: x => x.TestCodeId,
                        principalTable: "testCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification_Classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notification_Classes_classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notification_Classes_notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_Classes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentProfilePhoto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_Classes_classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_student_Classes_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "exam_Room_TestCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Exam_RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_Room_TestCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_Room_TestCodes_exam_Rooms_Exam_RoomId",
                        column: x => x.Exam_RoomId,
                        principalTable: "exam_Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_exam_Room_TestCodes_testCodes_TestCodeId",
                        column: x => x.TestCodeId,
                        principalTable: "testCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "testQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_testQuestionAnswers_testQuestions_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "testQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_Room_Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckinImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChenkTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExamRoomTestCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Exam_Room_TestCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_Room_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_Room_Students_exam_Room_TestCodes_Exam_Room_TestCodeId",
                        column: x => x.Exam_Room_TestCodeId,
                        principalTable: "exam_Room_TestCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_exam_Room_Students_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_Room_Student_AnswerHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamRoomStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestQuestionAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Exam_Room_StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_Room_Student_AnswerHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_Room_Student_AnswerHistories_exam_Room_Students_Exam_Room_StudentId",
                        column: x => x.Exam_Room_StudentId,
                        principalTable: "exam_Room_Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_exam_Room_Student_AnswerHistories_testQuestionAnswers_TestQuestionAnswerId",
                        column: x => x.TestQuestionAnswerId,
                        principalTable: "testQuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "examHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExamRoomStudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Exam_Room_StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_examHistories_exam_Room_Students_Exam_Room_StudentId",
                        column: x => x.Exam_Room_StudentId,
                        principalTable: "exam_Room_Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_classes_GradeId",
                table: "classes",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_classes_TeacherId",
                table: "classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_Student_AnswerHistories_Exam_Room_StudentId",
                table: "exam_Room_Student_AnswerHistories",
                column: "Exam_Room_StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_Student_AnswerHistories_TestQuestionAnswerId",
                table: "exam_Room_Student_AnswerHistories",
                column: "TestQuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_Students_Exam_Room_TestCodeId",
                table: "exam_Room_Students",
                column: "Exam_Room_TestCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_Students_StudentId",
                table: "exam_Room_Students",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_TestCodes_Exam_RoomId",
                table: "exam_Room_TestCodes",
                column: "Exam_RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Room_TestCodes_TestCodeId",
                table: "exam_Room_TestCodes",
                column: "TestCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Rooms_ExamId",
                table: "exam_Rooms",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Rooms_RoomId",
                table: "exam_Rooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Rooms_TeacherId1",
                table: "exam_Rooms",
                column: "TeacherId1");

            migrationBuilder.CreateIndex(
                name: "IX_exam_Rooms_TeacherId2",
                table: "exam_Rooms",
                column: "TeacherId2");

            migrationBuilder.CreateIndex(
                name: "IX_examHistories_Exam_Room_StudentId",
                table: "examHistories",
                column: "Exam_Room_StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_exams_SubjectId",
                table: "exams",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_Classes_ClassId",
                table: "notification_Classes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_Classes_NotificationId",
                table: "notification_Classes",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_student_Classes_ClassId",
                table: "student_Classes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_student_Classes_StudentId",
                table: "student_Classes",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_students_UserId",
                table: "students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_Grades_GradeId",
                table: "subjects_Grades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_Grades_SubjectId",
                table: "subjects_Grades",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_Subjects_SubjectId",
                table: "teacher_Subjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_Subjects_TeacherId",
                table: "teacher_Subjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_UserId",
                table: "teachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_testCodes_TestId",
                table: "testCodes",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_testQuestionAnswers_TestQuestionId",
                table: "testQuestionAnswers",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_testQuestions_TestCodeId",
                table: "testQuestions",
                column: "TestCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_tests_SubjectId",
                table: "tests",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_Room_Student_AnswerHistories");

            migrationBuilder.DropTable(
                name: "examHistories");

            migrationBuilder.DropTable(
                name: "notification_Classes");

            migrationBuilder.DropTable(
                name: "student_Classes");

            migrationBuilder.DropTable(
                name: "subjects_Grades");

            migrationBuilder.DropTable(
                name: "systemConfigs");

            migrationBuilder.DropTable(
                name: "teacher_Subjects");

            migrationBuilder.DropTable(
                name: "testQuestionAnswers");

            migrationBuilder.DropTable(
                name: "exam_Room_Students");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "testQuestions");

            migrationBuilder.DropTable(
                name: "exam_Room_TestCodes");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "exam_Rooms");

            migrationBuilder.DropTable(
                name: "testCodes");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
