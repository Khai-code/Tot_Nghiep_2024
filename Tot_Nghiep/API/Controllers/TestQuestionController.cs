using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Immutable;
using System.Linq;
using System.Net.WebSockets;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionController : ControllerBase
    {
        AppDbContext _Dbcontext;
        public TestQuestionController(AppDbContext appDbContext)
        {
            _Dbcontext = appDbContext;
        }
        [HttpGet("GetAll_TestQuestionAnswers")]
        public IActionResult Get_Questions()
        {
            return Ok(_Dbcontext.testQuestionAnswers.ToList());
        }
        [HttpGet("get-question-details/{id}")]
        public async Task<List<listdetailquestion>> GetQuestionDetails(Guid id)
        {
            var questionDetails = await _Dbcontext.testQuestions
                .Where(q => q.TestId == id)
                .Select(q => new listdetailquestion
                {
                    Id = q.Id,
                    Questionname = q.QuestionName,
                    RightAnswer = q.RightAnswer,
                    answer = q.TestQuestionAnswer.Select(a => new AnswerDTO
                    {
                        Answer = a.Answer,
                    }).ToList()
                }).ToListAsync();

            return questionDetails;
        }

        [HttpGet("GetAll_Question")]
        public async Task<ActionResult<List<ListQuestionDTO>>> Get_Question()
        {
            var list = await _Dbcontext.testQuestions.Include(tc=>tc.TestQuestionAnswer)
                .Include(tc => tc.Tests)

                        .ThenInclude(t => t.Subject)
                            .ThenInclude(s => s.Subject_Grade)
                                .ThenInclude(sg => sg.Grade)
                .ToListAsync();
            var groupedData = list.GroupBy(tc => new
            {
               idnew= tc.Tests,
                id=tc.Tests.Id,
                name=tc.CreatedByName,
                 tc.Tests.Id,
                GradeName = tc.Tests.Subject.Subject_Grade.FirstOrDefault().Grade.Name,
                TestName = tc.Tests.Name,
                SubjectName = tc.Tests.Subject.Name,
                code=tc.Tests.Code,
                
                        })
                 .Select(group => new ListQuestionDTO
                 {
                     id=group.Key.Id,
                     //code= group.Key.code,
                     namegrade = group.Key.GradeName,
                     nametest = group.Key.TestName,
                     name = group.Key.SubjectName,
                     usermane=group.Key.name,
                     totalquestion = group.Count() ,
                    
                 })
                 .ToList();

            return Ok(groupedData);

        }
        [HttpGet("GetAll_TestQuestion")]
        public async Task<ActionResult<List<TesCodeDTO>>> Get_TestQuestion()
        {
            try
            {
                var testCodes = await _Dbcontext.testCodes
                    .Include(tc => tc.Tests) 
                    .ToListAsync();
                var testDTOs = testCodes.Select(tc => new TesCodeDTO
                {
                    Id = tc.Id,         
                    Name = tc.Tests.Name 
                }).ToList();

                return Ok(testDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi máy chủ: {ex.Message}");
            }

        }


        [HttpPost("CreateQuestionWithAnswers")]
        public async Task<ActionResult> CreateQuestionWithAnswers(TestQuestionDTO testQuestionDTO)
        {
            try
            {
                var username =  User.Claims.FirstOrDefault(c => c.Type == "name");
                
                var testQuestion = new TestQuestion
                {
                    Id = Guid.NewGuid(),
                    QuestionName = testQuestionDTO.QuestionName,
                    Type = testQuestionDTO.Type,
                    RightAnswer = testQuestionDTO.RightAnswer,
                    TestId = testQuestionDTO.TestId ,
                    CreatedByName=testQuestionDTO.CreatedByName,
                };

                await _Dbcontext.testQuestions.AddAsync(testQuestion);

                if (testQuestionDTO.Answers != null && testQuestionDTO.Answers.Count > 0)
                {
                    foreach (var answerDTO in testQuestionDTO.Answers)
                    {
                        var testAnswer = new TestQuestionAnswer
                        {
                            Id = Guid.NewGuid(),
                            Answer = answerDTO.Answer, 
                            TestQuestionId = testQuestion.Id 
                        };

                        await _Dbcontext.testQuestionAnswers.AddAsync(testAnswer);
                    }
                }
                await _Dbcontext.SaveChangesAsync();
                return Ok("Tạo câu hỏi và câu trả lời thành công");
            }
            catch (Exception)
            {
                return BadRequest("Tạo câu hỏi và câu trả lời lỗi");
            }
        }
        [HttpPut("Update_TestQuestion")]
        public async Task<ActionResult> UpdateTestQuestion(TestQuestionDTO testQuestionDTO)
        {
            try
            {
                
                var updateQuestion = _Dbcontext.testQuestions.FirstOrDefault(temp => temp.Id == testQuestionDTO.Id);
                if (updateQuestion != null)
                {
                  
                    updateQuestion.QuestionName = testQuestionDTO.QuestionName;
                    updateQuestion.Type = testQuestionDTO.Type;
                    updateQuestion.RightAnswer = testQuestionDTO.RightAnswer;
                    updateQuestion.TestId = testQuestionDTO.TestId;

                    if (testQuestionDTO.Answers != null && testQuestionDTO.Answers.Count > 0)
                    {
                        foreach (var answerDTO in testQuestionDTO.Answers)
                        {
                            var existingAnswer = _Dbcontext.testQuestionAnswers
                                .FirstOrDefault(a => a.Id == answerDTO.Id);

                            if (existingAnswer != null)
                            {
                                existingAnswer.Answer = answerDTO.Answer;
                            }
                            else
                            {
                                var newAnswer = new TestQuestionAnswer
                                {
                                    Id = Guid.NewGuid(),
                                    Answer = answerDTO.Answer,
                                    TestQuestionId = updateQuestion.Id
                                };
                                await _Dbcontext.testQuestionAnswers.AddAsync(newAnswer);
                            }
                        }
                    }

                    _Dbcontext.testQuestions.Update(updateQuestion);
                    await _Dbcontext.SaveChangesAsync();
                    return Ok("Cập nhật thành công câu hỏi và câu trả lời.");
                }

                return NotFound("Câu hỏi không tồn tại.");
            }
            catch (Exception)
            {
                return BadRequest("Cập nhật thất bại.");
            }
        }

        [HttpDelete("Delete_TestQuestion")]
        public async Task<ActionResult> Delete_question(Guid Id)
        {
            var delete= _Dbcontext.testQuestions.FirstOrDefault(temp => temp.Id== Id);
            if (delete != null)
            {
                _Dbcontext.testQuestions.Remove(delete);
                await _Dbcontext.SaveChangesAsync();
                return Ok("Xóa thành công");
            }
            return BadRequest("Xóa thất bại");
        }
        [HttpGet("export")]
        public IActionResult ExportToExcel()
        {
            // Lấy dữ liệu từ bảng Questions và Answers
            var testQuestions = _Dbcontext.testQuestions.ToList();
            var testAnswers = _Dbcontext.testQuestionAnswers.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("TestQuestions");

                // Thêm tiêu đề cột
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Câu hỏi";
                worksheet.Cells[1, 3].Value = "Câu A";
                worksheet.Cells[1, 4].Value = "Câu B";
                worksheet.Cells[1, 5].Value = "Câu C";
                worksheet.Cells[1, 6].Value = "Câu D";
                worksheet.Cells[1, 7].Value = "Câu đúng";

                // Điền dữ liệu vào các ô
                int row = 2;
                foreach (var question in testQuestions)
                {
                    worksheet.Cells[row, 1].Value = question.Id;
                    worksheet.Cells[row, 2].Value = question.QuestionName;

                    // Lấy danh sách câu trả lời cho từng câu hỏi
                    var answers = testAnswers.Where(a => a.TestQuestionId == question.Id).ToList();

                    // Đặt câu trả lời vào các cột tương ứng
                    for (int i = 0; i < answers.Count && i < 4; i++)
                    {
                        worksheet.Cells[row, 3 + i].Value = answers[i].Answer;
                    }

                    // Thêm câu trả lời đúng vào cột cuối
                    worksheet.Cells[row, 7].Value = question.RightAnswer;

                    row++;
                }

                // Định dạng file Excel
                worksheet.Cells["A1:G1"].Style.Font.Bold = true;
                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TestQuestions.xlsx");
            }
        }
      
        [HttpPost("import")]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("File không hợp lệ.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy worksheet đầu tiên
                    var rowCount = worksheet.Dimension.Rows;

                    using (var transaction = await _Dbcontext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                // Khởi tạo dữ liệu từ file Excel
                                //var Id= worksheet.Cells[row, 1].Value?.ToString();
                                var questionName = worksheet.Cells[row, 2].Value?.ToString();
                                var rightAnswer = worksheet.Cells[row, 7].Value?.ToString();
                                var type = worksheet.Cells[row, 8].Value?.ToString();
                                var answers = new List<string>();

                                if (string.IsNullOrWhiteSpace(questionName) || string.IsNullOrWhiteSpace(rightAnswer) || string.IsNullOrWhiteSpace(type))
                                {
                                    return BadRequest($"Dữ liệu không hợp lệ tại dòng {row}. Vui lòng kiểm tra file Excel.");
                                }

                                if (!int.TryParse(type, out int parsedType))
                                {
                                    return BadRequest($"Giá trị không hợp lệ cho trường Type tại dòng {row}.");
                                }

                                for (int col = 3; col <= 6; col++)
                                {
                                    var answer = worksheet.Cells[row, col].Value?.ToString();
                                    if (!string.IsNullOrEmpty(answer))
                                    {
                                        answers.Add(answer);
                                    }
                                }
                                var testCodeId = Guid.Parse(""); 
                                var testCode = await _Dbcontext.testCodes
                                    .FirstOrDefaultAsync(tc => tc.Id == testCodeId);

                                var question = new TestQuestion
                                {
                                    Id = Guid.NewGuid(),
                                    QuestionName = questionName,
                                    Type = int.Parse(type),
                                    RightAnswer = rightAnswer,
                                    CreatedByName = "ninh minh quang",
                                    TestId = testCode.Id
                                };

                                await _Dbcontext.testQuestions.AddAsync(question);
                             
                                foreach (var answer in answers)
                                {
                                    var answerEntity = new TestQuestionAnswer
                                    {
                                        TestQuestionId = question.Id,
                                        Answer = answer
                                    };
                                    await _Dbcontext.testQuestionAnswers.AddAsync(answerEntity);
                                }
                            }

                            // Lưu và commit transaction
                            await _Dbcontext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                            return StatusCode(500, $"Lỗi khi nhập dữ liệu: {innerExceptionMessage}");
                        }
                    }
                }
            }

            return Ok("Dữ liệu đã được nhập thành công.");
        }


    }
}
