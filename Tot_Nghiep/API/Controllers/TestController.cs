using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.WebSockets;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        AppDbContext _DbContext;
        public TestController(AppDbContext appDbContext)
        {
            _DbContext = appDbContext;
        }
        private string RandomCode(int length)
        {
            const string CodeNew = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = CodeNew[random.Next(CodeNew.Length)];
            }

            return new string(code);
        }
        [HttpGet("Get_TetstCode")]
        public ActionResult Get()
        {
            return Ok(_DbContext.testCodes.ToList());
        }
        [HttpGet("GetALl_Test")]
        public ActionResult GetALL_Test()
        {
            //var test = _DbContext.tests.ToList();
            //var testcode = _DbContext.testCodes.ToList();
            //var resutl = new
            //{
            //    test = test,
            //    testcode = testcode
            //};
            return Ok(_DbContext.tests.ToList());
            //var results = _DbContext.tests
            //.Include(t => t.TestCodes) // Eager loading bảng test_code liên quan
            //.ToList();

            //return Ok(results);

        }
        [HttpPost("Post_Test")]
        public async Task<ActionResult> Post_Test(TestDTO testDTO)
        {
            var subject = await _DbContext.subjects.FirstOrDefaultAsync(temp => temp.Id == testDTO.SubjectId);

            if (subject == null)
            {
                return NotFound("Subject not found.");
            }

            using var transaction = await _DbContext.Database.BeginTransactionAsync();
            try
            {
                Test test = new Test
                {
                    Id = Guid.NewGuid(),
                    Type = testDTO.Type,
                    CreationTime = testDTO.CreationTime,
                    Status = 1,
                    SubjectId = subject.Id
                };

                await _DbContext.tests.AddAsync(test);
                await _DbContext.SaveChangesAsync();

                TestCode testCode = new TestCode
                {
                    Id = Guid.NewGuid(),
                    Code = RandomCode(8),
                    Status = 1,
                    TestId = test.Id,
                };

                await _DbContext.testCodes.AddAsync(testCode);
                await _DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(testCode);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("Update_Test")]
        public async Task<ActionResult> Update_Test(TestDTO testDTO)
        {
            var update = _DbContext.tests.FirstOrDefault(temp => temp.Id == testDTO.Id);
            if (update != null)
            {
                update.Type = testDTO.Type;
                update.CreationTime = testDTO.CreationTime;
                update.Status = testDTO.Status;
                update.SubjectId = testDTO.SubjectId;
                _DbContext.tests.Update(update);
                await _DbContext.SaveChangesAsync();
                return Ok("update thành công");
            }
            return BadRequest("Update failed");
        }
        [HttpPut("Update_TestCode")]
        public async Task<ActionResult> Update_TestCode(TestCode testCode)
        {
           var code= _DbContext.testCodes.FirstOrDefault(temp=>temp.Id == testCode.Id);
            if (code != null)
            {
                code.Code = RandomCode(8);
                code.Status = testCode.Status;
                code.TestId = testCode.Id;
                _DbContext.testCodes.Update(code);
                await _DbContext.SaveChangesAsync();
                return Ok("update thành công");
            }
            return BadRequest("update thất bại");
        }
        [HttpDelete("Delete_Test")]
        public async Task<ActionResult> Delete_Test(Guid Id)
        {
            var delete= _DbContext.tests.FirstOrDefault(x => x.Id == Id);
            if(delete != null)
            {
                _DbContext.tests.Remove(delete);
              await  _DbContext.SaveChangesAsync();
                return Ok("xóa thành công");
            }
            return BadRequest("xóa thất bại");
        }
    }
}
