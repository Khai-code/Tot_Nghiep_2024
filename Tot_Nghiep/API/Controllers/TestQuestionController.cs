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
        [HttpGet("GetAll_TestQuestionAnswers/{id}")]
        public async Task<List<TestQuestion_TestQuestionAnswersDTO>> Get_Questions(Guid id)
        {
            var result = await _Dbcontext.testQuestions
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.QuestionName,
                    a.Level,
                    a.TestId,
                    a.Type,
                    a.RightAnswer, 
                    Answers = a.TestQuestionAnswer.Select(c => c.Answer).ToList()
                })
                .ToListAsync();
            var dtoResult = result.Select(a => new TestQuestion_TestQuestionAnswersDTO
            {
                id = a.Id,
                QuestionName = a.QuestionName,
                Level = a.Level,
                TestId = a.TestId ?? Guid.Empty,
                QuestionType = a.Type,
                CorrectAnswers = string.IsNullOrEmpty(a.RightAnswer) ? new List<string>() : a.RightAnswer.Split(',').ToList(),
                Answers = a.Answers
            }).ToList();

            return dtoResult;
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
                    Type = q.Type,
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
            var list = await _Dbcontext.testQuestions
                .Include(tc=>tc.TestQuestionAnswer)
                    .Include(tc => tc.Tests)
                        .ThenInclude(t => t.Subject)
                            .ThenInclude(s => s.Subject_Grade)
                                .ThenInclude(sg => sg.Grade)
                .ToListAsync();
            var groupedData = list.GroupBy(tc => new
            {
               idnew= tc.Tests,
               type= tc.Type,
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
                     type=group.Key.type
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
        
    }
}
