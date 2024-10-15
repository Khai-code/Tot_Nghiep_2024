using System.Net.WebSockets;
using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        [HttpGet("search-student-result")]
        public async Task<ActionResult<IEnumerable<StudentExamResultDTO>>> GetStudentExamResult(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Keyword is required.");
            }
            // Tìm kiếm sinh viên theo Code hoặc Name và ánh xạ sang DTO
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
                                where student.Code.Contains(keyword) || student.User.FullName.Contains(keyword) /*|| classEntity.Name.Contains(keyword) || testCode.Code.Contains(keyword) || subject.Name.Contains(keyword)*/
                                select new StudentExamResultDTO
                                {
                                    StudentID = student.Id,
                                    examRoomTestCodeId = examRoomTestCode.Id,
                                    GradeName = grade.Name,
                                    ClassName = classEntity.Name,
                                    StudentCode = student.Code,
                                    StudentName = student.User.FullName,
                                    SubjectName = subject.Name,
                                    TestCode = testCode.Code,
                                    RoomName = examRoom.Room.Name,
                                    Answer = testQuestionAnswer.Answer,
                                    RightAnswer = testQuestion.RightAnswer,
                                    ExamTime = examHistory.CreationTime
                                }).ToListAsync();

            var studentResult = result.GroupBy(x => new {x.GradeName, x.ClassName, x.StudentID, x.examRoomTestCodeId, x.StudentCode, x.StudentName, x.SubjectName, x.TestCode, x.RoomName, x.ExamTime })
                .Select(g => new StudentExamResultDTO
                {
                    StudentID = g.Key.StudentID,
                    examRoomTestCodeId = g.Key.examRoomTestCodeId,
                    GradeName = g.Key.GradeName,
                    ClassName = g.Key.ClassName,
                    StudentCode = g.Key.StudentCode,
                    StudentName = g.Key.StudentName,
                    SubjectName = g.Key.SubjectName,
                    TestCode = g.Key.TestCode,
                    RoomName = g.Key.RoomName,
                    ExamTime = g.Key.ExamTime,
                    CorrectAnswers = g.Count(x => x.Answer == x.RightAnswer),
                    WrongAnswers = g.Count(x => x.Answer != x.RightAnswer),
                }).ToList();

            return Ok(studentResult);
        }

        [HttpGet("get-student-question")]
        public async Task<ActionResult> GetStudentQuestion(Guid studentId, Guid examRoomTestCodeId)
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
                                where student.Id == studentId && examRoomTestCode.Id == examRoomTestCodeId
                                select new TestQuestion_TestQuestionAnswersDTO
                                {
                                    QuestionName = testQuestion.QuestionName,
                                    QuestionType = testQuestion.Type,
                                    Level = testQuestion.Level,
                                    CreatedByName = testQuestion.CreatedByName,
                                    TestId = testQuestion.Tests.Id,
                                    Answers = (from tq in _db.testQuestions
                                               join tqAnswer in _db.testQuestionAnswers on tq.Id equals tqAnswer.TestQuestionId
                                               where tq.Id == testQuestion.Id
                                               select tqAnswer.Answer).ToList(),
                                    CorrectAnswers = (from tq in _db.testQuestions
                                                      where tq.RightAnswer == testQuestionAnswer.Answer && tq.Id == testQuestion.Id
                                                      select testQuestionAnswer.Answer).ToList(),

                                }).ToListAsync();

            return Ok(result);
        }

    }

}

