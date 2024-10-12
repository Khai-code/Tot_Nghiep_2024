using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

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

                    var newQuestion_type1 = new TestQuestion
                    {
                        Id = Guid.NewGuid(),
                        QuestionName = dto.QuestionName,
                        Type = dto.QuestionType,
                        Level = dto.Level,
                        CreatedByName = dto.CreatedByName,
                        TestId = dto.TestId,
                        RightAnswer = dto.CorrectAnswers.FirstOrDefault() // Lưu đáp án đúng
                    };

                    _db.testQuestions.Add(newQuestion_type1);

                    var answers = new List<TestQuestionAnswer>();
                    foreach (var answer in dto.Answers)
                    {
                        answers.Add(new TestQuestionAnswer
                        {
                            Id = Guid.NewGuid(),
                            Answer = answer,
                            TestQuestionId = newQuestion_type1.Id
                        });
                    }

                    // Lưu đáp án
                    _db.testQuestionAnswers.AddRange(answers);
                    await _db.SaveChangesAsync();

                    break;

                case 2: // Multiple Answers (Nhiều đáp án đúng)
                    if (!dto.CorrectAnswers.All(ca => dto.Answers.Contains(ca)))
                    {
                        throw new ArgumentException("Tất cả đáp án đúng phải nằm trong danh sách đáp án.");
                    }

                    var newQuestion_type2 = new TestQuestion
                    {
                        Id = Guid.NewGuid(),
                        QuestionName = dto.QuestionName,
                        Type = dto.QuestionType,
                        Level = dto.Level,
                        CreatedByName = dto.CreatedByName,
                        TestId = dto.TestId,
                        RightAnswer = string.Join(", ", dto.CorrectAnswers) // Lưu tất cả đáp án đúng
                    };

                    // Lưu đáp án
                    _db.testQuestions.Add(newQuestion_type2);

                    var answers2 = new List<TestQuestionAnswer>();
                    foreach (var answer in dto.Answers)
                    {
                        answers2.Add(new TestQuestionAnswer
                        {
                            Id = Guid.NewGuid(),
                            Answer = answer,
                            TestQuestionId = newQuestion_type2.Id
                        });
                    }

                    // Lưu đáp án
                    _db.testQuestionAnswers.AddRange(answers2);
                    await _db.SaveChangesAsync();

                    break;

                case 3: // True/False
                    dto.Answers = new List<string> { "True", "False" };
                    if (!dto.CorrectAnswers.Contains("True") && !dto.CorrectAnswers.Contains("False"))
                    {
                        throw new ArgumentException("Đáp án đúng phải là True hoặc False.");
                    }

                    var newQuestion_TrueFalse = new TestQuestion
                    {
                        Id = Guid.NewGuid(),
                        QuestionName = dto.QuestionName,
                        Type = dto.QuestionType,
                        Level = dto.Level,
                        CreatedByName = dto.CreatedByName,
                        TestId = dto.TestId,
                        RightAnswer = dto.CorrectAnswers.FirstOrDefault() // Lưu đáp án đúng
                    };

                    // Lưu đáp án
                    _db.testQuestions.Add(newQuestion_TrueFalse);

                    var answers3 = new List<TestQuestionAnswer>();
                    foreach (var answer in dto.Answers)
                    {
                        answers3.Add(new TestQuestionAnswer
                        {
                            Id = Guid.NewGuid(),
                            Answer = answer,
                            TestQuestionId = newQuestion_TrueFalse.Id
                        });
                    }

                    // Lưu đáp án
                    _db.testQuestionAnswers.AddRange(answers3);
                    await _db.SaveChangesAsync();

                    break;

                case 4: // Fill in the Blank
                        // Không cần đáp án cố định
                    var newFillInTheBlankQuestion = new TestQuestion
                    {
                        Id = Guid.NewGuid(),
                        QuestionName = dto.QuestionName,
                        Type = dto.QuestionType,
                        Level = dto.Level,
                        CreatedByName = dto.CreatedByName,
                        TestId = dto.TestId,
                        RightAnswer = string.Join(", ", dto.CorrectAnswers) // Lưu đáp án đúng (nếu có)
                    };

                    _db.testQuestions.Add(newFillInTheBlankQuestion);

                    await _db.SaveChangesAsync();

                    break;

                default:
                    throw new ArgumentException("Loại câu hỏi không hợp lệ.");
            }


            _db.SaveChanges();

            return Ok("thêm câu hỏi thành công");
        }

        [HttpPost("random-question-testcode")]
        public async Task<IActionResult> TestCodeQuestion(TestCodeQuestionDTO dto)
        {
            var testcodes = await _db.testCodes.Where(x => x.TestId == dto.TestId).ToListAsync();

            var easyQuestions = await _db.testQuestions.Where(x => x.TestId == dto.TestId && x.Level == 1).ToListAsync();
            var mediumQuestions = await _db.testQuestions.Where(x => x.TestId == dto.TestId && x.Level == 2).ToListAsync();
            var hardQuestions = await _db.testQuestions.Where(x => x.TestId == dto.TestId && x.Level == 3).ToListAsync();
            var advancedQuestions = await _db.testQuestions.Where(x => x.TestId == dto.TestId && x.Level == 4).ToListAsync();

            if (easyQuestions.Count < dto.EasyCount || mediumQuestions.Count < dto.MediumCount || 
                hardQuestions.Count < dto.HardCount || advancedQuestions.Count < dto.AdvancedCount)
            {
                return NotFound("không đủ số câu hỏi cho1 hoặc nhiều mức độ");
            }

            Random random = new Random();

            foreach (var testcode in testcodes)
            {
                var selectedEasyQuestions = easyQuestions.OrderBy(x => random.Next()).Take(dto.EasyCount).ToList();
                var selectedMediumQuestions = mediumQuestions.OrderBy(x => random.Next()).Take(dto.MediumCount).ToList();
                var selectedHardQuestions = hardQuestions.OrderBy(x => random.Next()).Take(dto.HardCount).ToList(); 
                var selectedAdvancedQuestions = advancedQuestions.OrderBy(x => random.Next()).Take(dto.AdvancedCount).ToList();

                var allSelectedQuestions = selectedEasyQuestions
                    .Concat(selectedMediumQuestions)
                    .Concat(selectedHardQuestions)
                    .Concat(selectedAdvancedQuestions)
                    .ToList();

                foreach (var question in allSelectedQuestions)
                {
                    _db.TestCode_TestQuestions.Add(new TestCode_TestQuestion
                    {
                        TestCodeId = testcode.Id,
                        TestQuestionId = question.Id
                    });
                }
            }

            await _db.SaveChangesAsync();

            return Ok("Chia câu hỏi vào mã đề thành công");
        }
    }
}
