using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionDTOController : ControllerBase
    {
        AppDbContext _db;
        public TestQuestionDTOController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("create_question_answwer")]
        public async Task<IActionResult> QuestionWithAnswers(TestQuestion_TestQuestionAnswersDTO dto)
        {
            // Kiểm tra loại câu hỏi và xử lý tương ứng
            switch (dto.QuestionType)
            {
                case 1: // Multiple Choice (Một đáp án đúng)
                    if (!dto.Answers.Contains(dto.CorrectAnswers.FirstOrDefault()))
                    {
                        throw new ArgumentException("Đáp án đúng phải nằm trong danh sách đáp án.");
                    }
                    break;
                case 2: // Multiple Answers (Nhiều đáp án đúng)
                    if (!dto.CorrectAnswers.All(ca => dto.Answers.Contains(ca)))
                    {
                        throw new ArgumentException("Tất cả đáp án đúng phải nằm trong danh sách đáp án.");
                    }
                    break;
                case 3: // True/False
                    dto.Answers = new List<string> { "True", "False" };
                    if (!dto.CorrectAnswers.Contains("True") && !dto.CorrectAnswers.Contains("False"))
                    {
                        throw new ArgumentException("Đáp án đúng phải là True hoặc False.");
                    }
                    break;
                case 4: // Fill in the Blank
                    dto.Answers = new List<string>(); // Không có đáp án cố định trong bảng đáp án
                    break;
                default:
                    throw new ArgumentException("Loại câu hỏi không hợp lệ.");
            }

            // Tạo mới câu hỏi
            var newQuestion = new TestQuestion
            {
                Id = Guid.NewGuid(),
                QuestionName = dto.QuestionName,
                Type = dto.QuestionType,
                CreatedByName = dto.CreatedByName,
                TestId = dto.TestId,
                RightAnswer = dto.CorrectAnswers.FirstOrDefault() // Đáp án đúng (cho câu hỏi điền chữ và True/False)
            };

            // Lưu câu hỏi
            _db.testQuestions.Add(newQuestion);
            await _db.SaveChangesAsync();

            // Xử lý đáp án (nếu có)
            if (dto.QuestionType == 1 || dto.QuestionType == 2)
            {
                var answers = new List<TestQuestionAnswer>();
                foreach (var answer in dto.Answers)
                {
                    answers.Add(new TestQuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        Answer = answer,
                        TestQuestionId = newQuestion.Id
                    });
                }

                // Lưu đáp án
                _db.testQuestionAnswers.AddRange(answers);
                await _db.SaveChangesAsync();
            }

            return Ok(newQuestion);
        }

    }
}
