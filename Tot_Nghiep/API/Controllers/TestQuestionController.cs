using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("GetAll_TesTQuestion")]
        public IActionResult Get_TestQuestion()
        {
            return Ok(_Dbcontext.testQuestions.ToList());
        }
        [HttpPost("CreateQuestionWithAnswers")]
        public async Task<ActionResult> CreateQuestionWithAnswers(TestQuestionDTO testQuestionDTO)
        {
            try
            {
                // Tạo câu hỏi mới
                var testQuestion = new TestQuestion
                {
                    Id = Guid.NewGuid(),
                    QuestionName = testQuestionDTO.QuestionName,
                    Type = testQuestionDTO.Type,
                    RightAnswer = testQuestionDTO.RightAnswer,
                    TestCodeId = testQuestionDTO.TestCodeId // Liên kết với TestCode
                };

                await _Dbcontext.testQuestions.AddAsync(testQuestion);

                // Tạo danh sách câu trả lời dựa trên số lượng câu trả lời mà người dùng nhập
                if (testQuestionDTO.Answers != null && testQuestionDTO.Answers.Count > 0)
                {
                    foreach (var answerDTO in testQuestionDTO.Answers)
                    {
                        var testAnswer = new TestQuestionAnswer
                        {
                            Id = Guid.NewGuid(),
                            Answer = answerDTO.Answer, // Lấy câu trả lời từ answerDTO
                            TestQuestionId = testQuestion.Id // Liên kết với câu hỏi
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
