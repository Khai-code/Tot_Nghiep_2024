using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using OfficeOpenXml;

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

        [HttpPost("import_questions")]
        public async Task<IActionResult> ImportQuestionsFromExcel(IFormFile file, Guid id)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Chọn một file Excel hợp lệ.");
            }

            var testCode = await _db.tests.FirstOrDefaultAsync(c => c.Id == id);
            var questionsList = new List<TestQuestion_TestQuestionAnswersDTO>();
            var errorMessages = new List<string>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string typeText = worksheet.Cells[row, 2].Text.Trim();
                        string levelText = worksheet.Cells[row, 4].Text.Trim();

                        int convertype(string typetext)
                        {
                            switch (typetext.ToLower())
                            {
                                case "trắc nghiệm 1 đáp án":
                                    return 1;
                                case "trắc nghiệm nhiều đáp án":
                                    return 2;
                                case "đúng/sai":
                                    return 3;
                                case "điền vào chỗ trống":
                                    return 4;
                                default:
                                    errorMessages.Add($"Giá trị type không hợp lệ ở hàng {row}: {typetext}");
                                    return -1; // Trả về -1 hoặc giá trị mặc định nào đó
                            }
                        }

                        int ConvertLevel(string levelText)
                        {
                            switch (levelText.ToLower())
                            {
                                case "dễ":
                                    return 1;
                                case "trung bình":
                                    return 2;
                                case "khó":
                                    return 3;
                                case "rất khó":
                                    return 4;
                                default:
                                    errorMessages.Add($"Giá trị Level không hợp lệ ở hàng {row}: {levelText}");
                                    return -1;
                            }
                        }

                        int questionType = convertype(typeText);
                        int level = ConvertLevel(levelText);

                        if (questionType == -1 || level == -1)
                        {
                            continue; 
                        }

                       //var username = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                        var dto = new TestQuestion_TestQuestionAnswersDTO
                        {
                            QuestionType = questionType,
                            QuestionName = worksheet.Cells[row, 3].Text,
                            Level = level,
                            CreatedByName= "",
                            TestId = testCode.Id,

                            CorrectAnswers = worksheet.Cells[row, 5].GetValue<string>()
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim())
                            .Where(c => !string.IsNullOrWhiteSpace(c))
                            .ToList(),

                            // Đọc các đáp án từ các ô cột khác nhau
                            Answers = new List<string>
                    {
                        worksheet.Cells[row, 6].Text,
                        worksheet.Cells[row, 7].Text,
                        worksheet.Cells[row, 8].Text,
                        worksheet.Cells[row, 9].Text,
                        worksheet.Cells[row, 10].Text,
                        worksheet.Cells[row, 11].Text
                    }.Where(x => !string.IsNullOrEmpty(x)).ToList()
                        };

                        questionsList.Add(dto);
                    }
                }
            }

            // Ghi chú nếu có lỗi
            if (errorMessages.Count > 0)
            {
                return BadRequest(string.Join("\n", errorMessages));
            }

            // Xử lý từng câu hỏi và lưu vào cơ sở dữ liệu
            foreach (var dto in questionsList)
            {
                await QuestionWithAnswers(dto);
            }

            return Ok("Nhập câu hỏi thành công từ file Excel.");
        }

    }
}
