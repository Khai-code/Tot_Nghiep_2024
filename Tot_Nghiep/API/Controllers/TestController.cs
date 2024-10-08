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


        private int GetMaxStudentForSpecificClass(string classCode, Guid subjectId)
        {
            // Lấy lớp học dựa trên mã lớp (classCode)
            var classEntity = _DbContext.classes.FirstOrDefault(c => c.Code == classCode);

            if (classEntity == null)
            {
                Console.WriteLine($"Không tìm thấy lớp với mã: {classCode}");
                return 0;
            }

            // Lấy GradeId của lớp học
            var gradeId = classEntity.GradeId;

            // Kiểm tra xem SubjectId có tồn tại trong bảng subjects_Grades hay không
            var subjectGrade = _DbContext.subjects_Grades.FirstOrDefault(sg => sg.GradeId == gradeId && sg.SubjectId == subjectId);

            if (subjectGrade == null)
            {
                Console.WriteLine($"Không tìm thấy SubjectId {subjectId} cho GradeId {gradeId}");
                return 0;
            }

            // Nếu tất cả khớp, trả về số lượng MaxStudent
            Console.WriteLine($"MaxStudent for class {classCode}: {classEntity.MaxStudent}");
            return classEntity.MaxStudent;
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

                // Lấy số lượng MaxStudent dựa trên classCode và SubjectId
                int maxStudents = GetMaxStudentForSpecificClass(testDTO.ClassCode, testDTO.SubjectId);
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
                    MaxStudent = maxStudents,
                };

                // Thêm thực thể Test vào DbContext
                await _DbContext.tests.AddAsync(newTest);
                await _DbContext.SaveChangesAsync();

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

                if (ex.InnerException != null)
                {
                    return BadRequest($"Lỗi khi tạo bài kiểm tra: {ex.InnerException.Message}");
                }

                // Bắt lỗi cụ thể và trả về phản hồi lỗi chi tiết
                return BadRequest($"Lỗi khi tạo bài kiểm tra: {ex.Message}");
            }
        }


    }
}
