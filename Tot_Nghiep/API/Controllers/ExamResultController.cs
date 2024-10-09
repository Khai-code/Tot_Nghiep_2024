using System.Net.WebSockets;
using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ExamResultController(AppDbContext db)
        {
            _db = db;
        }



        [HttpGet("get-student-result")]
        public async Task<ActionResult<List<StudentExamResultDTO>>> GetStudentExamResult()
        {
            var result = await (from student in _db.students
                                join studentClass in _db.student_Classes on student.Id equals studentClass.StudentId
                                join classEntity in _db.classes on studentClass.ClassId equals classEntity.Id
                                join grade in _db.grades on classEntity.GradeId equals grade.Id // Thêm nối qua bảng Grade
                                join subjectGrade in _db.subjects_Grades on grade.Id equals subjectGrade.GradeId // Nối qua bảng Subject-Grade
                                join subject in _db.subjects on subjectGrade.SubjectId equals subject.Id // Lấy Subject qua Subject-Grade
                                join exam in _db.exams on subject.Id equals exam.SubjectId
                                join examRoom in _db.exam_Rooms on exam.Id equals examRoom.ExamId
                                join examRoomTestCode in _db.exam_Room_TestCodes on examRoom.Id equals examRoomTestCode.ExamRoomId // Thêm bảng trung gian Exam-Room-TestCode
                                join examRoomStudent in _db.exam_Room_Students on examRoomTestCode.Id equals examRoomStudent.ExamRoomTestCodeId // Join qua ExamRoomTestCod
                                join examHistory in _db.examHistories on examRoomStudent.Id equals examHistory.ExamRoomStudentId // Join từ ExamRoomStudent đến ExamHistory
                                join testCode in _db.testCodes on examRoomTestCode.TestCodeId equals testCode.Id // Lấy mã đề thi từ TestCodes
                                join test in _db.tests on testCode.TestId equals test.Id // Thêm nối qua bảng Test
                                join testQuestion in _db.testQuestions on test.Id equals testQuestion.TestId // Nối TestQuestion với Test
                                join testQuestionAnswer in _db.testQuestionAnswers on testQuestion.Id equals testQuestionAnswer.TestQuestionId
                                select new StudentExamResultDTO
                                {
                                    StudentCode = student.Code,
                                    StudentName = student.User.FullName,                
                                    SubjectName = subject.Name,
                                    TestCode = testCode.Code,
                                    RoomName = examRoom.Room.Name,
                                    //CorrectAnswers = testQuestionAnswer. ? 1 : 0,
                                    //WrongAnswers = testQuestionAnswer.IsCorrect ? 0 : 1,
                                    Score = examHistory.Score,
                                    ExamTime = examHistory.CreationTime
                                }).ToListAsync();

            return Ok(result);
        }


    }
}
