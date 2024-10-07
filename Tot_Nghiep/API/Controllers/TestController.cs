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
                .Include(t => t.Tests)
                .Include(t => t.Tests.Subject)
                .Include(t=>t.Tests.PointType)
                .AsQueryable();
            
            var testList = await query
                .Select(t => new TestGridDTO
                {
                    Id = t.Tests.Id,
                    namepoint=t.Tests.PointType.Name,
                    Name = t.Tests.Name,
                    Code = t.Tests.Code.ToString(),
                    //NumberOfTestCode = t.Tests.NumberOfTestCode,
                    SubjectName = t.Tests.Subject.Name,
                    Status = t.Status,
                    SubjectId = t.Tests.SubjectId,
                    Minute = t.Tests.Minute,
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
                    //NumberOfTestCode = testDTO.NumberOfTestCode,
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
                //update.NumberOfTestCode = testDTO.NumberOfTestCode;
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

        private int GetMaxStudentForSpecificClass(string classCode)
        {
            var result = (from test in _DbContext.tests
                          join subjectGrade in _DbContext.subjects_Grades on test.SubjectId equals subjectGrade.SubjectId
                          join grade in _DbContext.grades on subjectGrade.GradeId equals grade.Id
                          join classEntity in _DbContext.classes on grade.Id equals classEntity.GradeId
                          where classEntity.Code == classCode // Điều kiện để chọn lớp cụ thể
                          select classEntity.MaxStudent).FirstOrDefault();

            return result;
        }


        [HttpPost("create-test-or-create-testcode")]
        public async Task<IActionResult> CreateTest_Testcode(TestDTO testDTO)
        {
            try
            {
                // Kiểm tra nếu classCode có tồn tại
                if (string.IsNullOrEmpty(testDTO.ClassCode))
                {
                    return BadRequest("ClassCode không được để trống.");
                }

                // Lấy số lượng MaxStudent dựa trên classCode
                int maxStudents = GetMaxStudentForSpecificClass(testDTO.ClassCode);
                if (maxStudents == 0)
                {
                    return BadRequest("Không tìm thấy số lượng sinh viên tối đa cho lớp học.");
                }

                // Tạo thực thể Test từ DTO
                var newTest = new Test
                {
                    Id = Guid.NewGuid(),
                    Name = testDTO.Name,
                    Code = RandomCodeTest(6), // Tạo mã ngẫu nhiên
                    CreationTime = DateTime.Now,
                    Minute = testDTO.Minute,
                    Status = testDTO.Status,
                    SubjectId = testDTO.SubjectId,
                    PointTypeId = testDTO.PointTypeId,
                };

                // Thêm thực thể Test vào DbContext
                await _DbContext.tests.AddAsync(newTest);

                // Tạo TestCode tương ứng với số lượng MaxStudent
                for (int i = 0; i < maxStudents; i++)
                {
                    var newTestCode = new TestCode
                    {
                        Id = Guid.NewGuid(),
                        Code = RandomCode(8), // Tạo mã ngẫu nhiên
                        Status = 1,
                        TestId = newTest.Id, // Gán TestId
                    };

                    // Thêm thực thể TestCode vào DbContext
                    await _DbContext.testCodes.AddAsync(newTestCode);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _DbContext.SaveChangesAsync();

                return Ok("Tạo bài kiểm tra và mã bài kiểm tra thành công.");
            }
            catch (Exception ex)
            {
                // Bắt lỗi cụ thể và trả về phản hồi lỗi chi tiết
                return BadRequest($"Lỗi khi tạo bài kiểm tra: {ex.Message}");
            }
        }

    }
}
