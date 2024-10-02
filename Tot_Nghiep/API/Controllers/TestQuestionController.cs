using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("GetAll_Question")]
        public async Task<ActionResult<List<ListQuestionDTO>>> Get_Question()
        {
            var list = await _Dbcontext.testQuestions
                .Include(tc => tc.TestCode)
                    .ThenInclude(tc => tc.Test)
                        .ThenInclude(t => t.Subject)
                            .ThenInclude(s => s.Subject_Grade)
                                .ThenInclude(sg => sg.Grade)
                .ToListAsync();
            var groupedData = list.GroupBy(tc => new
            {
                code=tc.TestCode.Code,
                name=tc.CreatedByName,
                tc.TestCodeId,
                GradeName = tc.TestCode.Test.Subject.Subject_Grade.FirstOrDefault().Grade.Name,
                TestName = tc.TestCode.Test.Name,
                SubjectName = tc.TestCode.Test.Subject.Name,
                
                        })
                 .Select(group => new ListQuestionDTO
                 {
                     
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
                    .Include(tc => tc.Test) 
                    .ToListAsync();
                var testDTOs = testCodes.Select(tc => new TesCodeDTO
                {
                    Id = tc.Id,         
                    Name = tc.Test.Name 
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
                    TestCodeId = testQuestionDTO.TestCodeId ,
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
                // Tìm câu hỏi cần cập nhật
                var updateQuestion = _Dbcontext.testQuestions.FirstOrDefault(temp => temp.Id == testQuestionDTO.Id);
                if (updateQuestion != null)
                {
                    // Cập nhật thông tin câu hỏi
                    updateQuestion.QuestionName = testQuestionDTO.QuestionName;
                    updateQuestion.Type = testQuestionDTO.Type;
                    updateQuestion.RightAnswer = testQuestionDTO.RightAnswer;
                    updateQuestion.TestCodeId = testQuestionDTO.TestCodeId;

                    // Cập nhật từng câu trả lời
                    if (testQuestionDTO.Answers != null && testQuestionDTO.Answers.Count > 0)
                    {
                        foreach (var answerDTO in testQuestionDTO.Answers)
                        {
                            // Tìm câu trả lời trong database dựa trên ID của nó
                            var existingAnswer = _Dbcontext.testQuestionAnswers
                                .FirstOrDefault(a => a.Id == answerDTO.Id);

                            if (existingAnswer != null)
                            {
                                // Nếu câu trả lời đã tồn tại, cập nhật thông tin
                                existingAnswer.Answer = answerDTO.Answer;
                            }
                            else
                            {
                                // Nếu câu trả lời chưa tồn tại, thêm mới
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
    }
}
