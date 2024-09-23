using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Student_Class> student_Classes { get; set; }
        public DbSet<SystemConfig> systemConfigs { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Notification_Class> notification_Classes { get; set; }
        public DbSet<Class> classes { get; set; }
        public DbSet<Grade> grades { get; set; }
        public DbSet<Subject> subjects { get; set; }
        public DbSet<Subject_Grade> subjects_Grades { get; set; }
        public DbSet<Exam> exams { get; set; }
        public DbSet<Exam_Room> exam_Rooms { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<ExamHistory> examHistories { get; set; }
        public DbSet<Exam_Room_Student> exam_Room_Students { get; set; }
        public DbSet<Exam_Room_Student_AnswerHistory> exam_Room_Student_AnswerHistories { get; set; }
        public DbSet<Test> tests { get; set; }
        public DbSet<TestCode> testCodes { get; set; }
        public DbSet<TestQuestion> testQuestions { get; set; }
        public DbSet<TestQuestionAnswer> testQuestionAnswers { get; set; }
        public DbSet<Exam_Room_TestCode> exam_Room_TestCodes { get; set; }
        public DbSet<Teacher_Subject> teacher_Subjects { get; set; }
    }
}
