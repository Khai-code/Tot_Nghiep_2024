using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
            return Ok(_DbContext.tests.ToList());
        }

        private int RandomCodeTest(int length)
        {
            const string CodeNew = "0123456789";

            Random random = new Random();

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = CodeNew[random.Next(CodeNew.Length)];
            }

            return int.Parse(code);
        }
        [HttpGet("GetAll_pointtype")]
        public ActionResult GetAll()
        {
            return Ok(_DbContext.pointTypes.ToList());
        }
        
        [HttpGet("get-detail-test/{id}")]
        public async Task<IActionResult> GetListTest([FromRoute] Guid id)
        {
            var test = await _DbContext.tests.FindAsync(id);
            return Ok(test);
        }
        [HttpGet("get-list-test")]
        public async Task<List<TestGridDTO>> GetListTest([FromQuery] GetListTestQueryDTO input)
        {
            var query = _DbContext.testCodes
                .Include(t => t.Test)
                .Include(t => t.Test.Subject)
                .Include(t=>t.Test.PointType)
                .AsQueryable();
            
            var testList = await query
                .Select(t => new TestGridDTO
                {
                    Id = t.Test.Id,
                    namepoint=t.Test.PointType.Name,
                    Name = t.Test.Name,
                    Code = t.Test.Code.ToString(),
                    NumberOfTestCode = t.Test.NumberOfTestCode,
                    SubjectName = t.Test.Subject.Name,
                    Status = t.Status,
                    SubjectId = t.Test.SubjectId,
                    Minute = t.Test.Minute,
                })
                .ToListAsync();

            return testList;
        }
        [HttpPost("Post_Test")]
        public async Task<ActionResult> Post_Test(TestDTO testDTO)
        {
            var subject = await _DbContext.subjects.FirstOrDefaultAsync(temp => temp.Id == testDTO.SubjectId);
            var point = await _DbContext.pointTypes.FirstOrDefaultAsync(temp => temp.Id == testDTO.PointTypeId);

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
                    Name = testDTO.Name,
                    Code = RandomCodeTest(6),
                    Minute = testDTO.Minute,
                    NumberOfTestCode = testDTO.NumberOfTestCode,
                    CreationTime = DateTime.Now,
                    Status = testDTO.Status,
                    SubjectId = subject.Id,
                    PointTypeId = testDTO.PointTypeId,
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
                  
                await _DbContext.testCodes.AddRangeAsync(testCode);
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
        public async Task<ActionResult> Update_Test([FromBody] TestDTO testDTO)
        {
            var update = _DbContext.tests.FirstOrDefault(temp => temp.Id == testDTO.Id);
            if (update != null)
            {
                update.Name = testDTO.Name;
                update.Code = testDTO.Code;
                update.Minute = testDTO.Minute;
                update.NumberOfTestCode = testDTO.NumberOfTestCode;
                update.CreationTime = testDTO.CreationTime;
                update.Status = testDTO.Status;
                update.SubjectId = testDTO.SubjectId;
                update.PointTypeId = testDTO.PointTypeId;
                _DbContext.tests.Update(update);
                await _DbContext.SaveChangesAsync();
                return Ok("update thành công");
            }
            return BadRequest("Update failed");
        }
        [HttpPost("post_pointype")]
        public async Task<ActionResult> create_pointtype(PointTypeDTO pointType)
        {
            try
            {
                PointType point = new PointType
                {
                    Id = Guid.NewGuid(),
                    Name = pointType.Name,
                };
                await _DbContext.pointTypes.AddAsync(point);
                await _DbContext.SaveChangesAsync();
                return Ok("thêm thành công");
            }catch (Exception)
            {
                return BadRequest("thêm thất bại");
            }
        }
        [HttpPut("Update_TestCode")]
        public async Task<ActionResult> Update_TestCode(TestCode testCode)
        {
            var code = _DbContext.testCodes.FirstOrDefault(temp => temp.Id == testCode.Id);
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
            var delete = _DbContext.tests.FirstOrDefault(x => x.Id == Id);
            if (delete != null)
            {
                _DbContext.tests.Remove(delete);
                await _DbContext.SaveChangesAsync();
                return Ok("xóa thành công");
            }
            return BadRequest("xóa thất bại");
        }
    }
}
