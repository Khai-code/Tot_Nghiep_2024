using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using OfficeOpenXml;
using System.Linq;
using OfficeOpenXml.Style;
using System.Reflection.PortableExecutable;
using Emgu.CV;

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
        [HttpGet("Get-testcodes-by-testid")]
        public async Task<ActionResult<List<DetailDTO>>> GetTestCodesByTestId(Guid testId)
        {

            var testcodes = await _db.testCodes
            .Where(tc => tc.TestId == testId)
            .Select(tc => new DetailDTO
            {
                IdTestcode = tc.Id,
                CodeTescode = tc.Code,
                time = tc.Tests.Minute,
                NameSubject = tc.Tests.Subject.Name,
                NameQuestion = _db.TestCode_TestQuestions
                    .Where(tcq => tcq.TestCodeId == tc.Id)
                    .Select(tcq => new TestQuestionDTO
                    {
                        Id = tcq.TestQuestion.Id,
                        QuestionName = tcq.TestQuestion.QuestionName,
                        code=tc.Tests.Code,
                        Level = tcq.TestQuestion.Level,
                        Type = tcq.TestQuestion.Type,
                        Answers = _db.testQuestionAnswers
                            .Where(a => a.TestQuestionId == tcq.TestQuestionId)
                            .Select(a => new AnswerDTO
                            {
                                Id = a.Id,
                                Answer = a.Answer
                            }).ToList()
                    }).ToList()
            }).ToListAsync();

            return testcodes;
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

        [HttpPost("randomize-questions-for-test-codes")]
        public async Task<IActionResult> RandomizeQuestionsForTestCodes(Guid testId, int easyCount, int mediumCount, int hardCount, int veryHardCount)
        {
            var allTestCodes = await _db.testCodes
                .Include(tc => tc.Tests)
                .Where(tc => tc.Tests.Id == testId)
                .ToListAsync();

            if (allTestCodes == null || allTestCodes.Count == 0)
            {
                return NotFound("Không tìm thấy mã kiểm tra liên quan đến bài thi.");
            }
            var questions = await _db.testQuestions.Where(x => x.TestId == testId).ToListAsync();
            var easyQuestions = questions.Where(x => x.Level == 1).ToList();
            var mediumQuestions = questions.Where(x => x.Level == 2).ToList();
            var hardQuestions = questions.Where(x => x.Level == 3).ToList();
            var veryHardQuestions = questions.Where(x => x.Level == 4).ToList();

            if (easyQuestions.Count < easyCount || mediumQuestions.Count < mediumCount ||
                hardQuestions.Count < hardCount || veryHardQuestions.Count < veryHardCount)
            {
                return BadRequest("Không đủ số câu hỏi cho một hoặc nhiều mức độ.");
            }

            Random random = new Random();

            var selectedEasyQuestions = easyQuestions.OrderBy(_ => random.Next()).Take(easyCount).ToList();
            var selectedMediumQuestions = mediumQuestions.OrderBy(_ => random.Next()).Take(mediumCount).ToList();
            var selectedHardQuestions = hardQuestions.OrderBy(_ => random.Next()).Take(hardCount).ToList();
            var selectedVeryHardQuestions = veryHardQuestions.OrderBy(_ => random.Next()).Take(veryHardCount).ToList();

       
            var allSelectedQuestions = selectedEasyQuestions
                .Concat(selectedMediumQuestions)
                .Concat(selectedHardQuestions)
                .Concat(selectedVeryHardQuestions)
                .ToList();

            foreach (var testCode in allTestCodes)
            {
                foreach (var question in allSelectedQuestions)
                {
                    var testCodeQuestion = new TestCode_TestQuestion
                    {
                        TestCodeId = testCode.Id,  
                        TestQuestionId = question.Id
                    };

                    _db.TestCode_TestQuestions.Add(testCodeQuestion);
                }
            }

            await _db.SaveChangesAsync();

            return Ok("THÀNH CÔNG");
        }


        [HttpGet("export-template")]
        public IActionResult ExportExcelTemplate()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Câu hỏi");

                var headers = new List<string>
                {
                    "STT", "Kiểu câu hỏi", "Nội dung câu hỏi", "Mức độ tư duy",
                    "Đáp án đúng", "Câu A", "Câu B", "Câu C", "Câu D", "Câu E", "Câu F"
                };

                // Ghi tiêu đề vào hàng đầu tiên
                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Thiết lập độ rộng cho các cột
                worksheet.Column(1).Width = 5;   // STT
                worksheet.Column(2).Width = 20;  // Kiểu câu hỏi
                worksheet.Column(3).Width = 30;  // Nội dung câu hỏi
                worksheet.Column(4).Width = 15;  // Mức độ tư duy
                worksheet.Column(5).Width = 15;  // Đáp án đúng
                worksheet.Column(6).Width = 10;  // Câu A
                worksheet.Column(7).Width = 10;  // Câu B
                worksheet.Column(8).Width = 10;  // Câu C
                worksheet.Column(9).Width = 10;  // Câu D
                worksheet.Column(10).Width = 10; // Câu E
                worksheet.Column(11).Width = 10; // Câu F
                using (var headerRange = worksheet.Cells[1, 1, 1, headers.Count])
                {
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.RoyalBlue);
                    headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    headerRange.Style.Font.Bold = true; 
                }

             
                var questionTypeList = new List<string> { "Trắc nghiệm 1 Đáp án", "Trắc nghiệm nhiều đáp án", "Đúng/sai", "Điền vào chỗ trống" };
                CreateDropdownList(worksheet, questionTypeList, 2, 2, 100, 2); // Áp dụng cho cột 2 (Kiểu câu hỏi)

                // Tạo dropdown list cho "Mức độ tư duy"
                var thinkingLevelList = new List<string> { "Dễ", "Trung bình", "Khó", "Rất khó" };
                CreateDropdownList(worksheet, thinkingLevelList, 2, 4, 100, 4); // Áp dụng cho cột 4 (Mức độ tư duy)

                // Lưu file vào MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MauCauHoi.xlsx");
            }
        }

        private void CreateDropdownList(ExcelWorksheet worksheet, List<string> options, int fromRow, int fromCol, int toRow, int toCol)
        {
            // Tạo một List Validation trong khoảng ô được chỉ định
            var validation = worksheet.DataValidations.AddListValidation(worksheet.Cells[fromRow, fromCol, toRow, toCol].Address);

            // Thêm các giá trị từ danh sách 'options' vào dropdown list
            foreach (var option in options)
            {
                validation.Formula.Values.Add(option);
            }

            // Cấu hình thêm cho dropdown list
            validation.ShowErrorMessage = true; // Hiển thị thông báo lỗi nếu nhập sai giá trị
            validation.ErrorTitle = "Giá trị không hợp lệ"; // Tiêu đề thông báo lỗi
            validation.Error = "Vui lòng chọn một giá trị từ danh sách."; // Nội dung thông báo lỗi
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
                                    return -1; 
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
