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
        [HttpGet("Coute-testcode")]
        public async Task<IActionResult> Coute_TestCode()
        {
            var coute = await _DbContext.testCodes.CountAsync();
            return Ok(coute);
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
        [HttpGet("{testId}/questions")]
            public async Task<IActionResult> GetQuestions(Guid testId, [FromQuery] int level)
            {
                var questions = await _DbContext.testQuestions
                    .Where(q => q.TestId == testId && q.Level == level)
                    .Select(q => new TestQuestion_TestQuestionAnswersDTO
                    {
                        TestId = q.Id,
                        QuestionName = q.QuestionName,
                        Level = q.Level,
                        CorrectAnswers= new List<string>(),
                    })
                    .ToListAsync();

                if (questions == null || !questions.Any())
                {
                    return NotFound(new { message = "Không có câu hỏi nào cho test này." });
                }

                return Ok(questions);
            }
        

        [HttpGet("get-list-test")]
        public async Task<List<TestGridDTO>> GetListTest()
        {
            var query = _DbContext.tests
                .Include(c=>c.testQuestions)
                .Include(c=>c.testCodes)
                .Include(t => t.Subject)
                .Include(t => t.Subject.Subject_Grade)
                .ThenInclude(t => t.Grade)
                .ThenInclude(g => g.Class)
                .Include(t => t.PointType)
                .AsQueryable();

            var testList = await query
                .Select(t => new TestGridDTO
                {
                    idquestion = t.testCodes.FirstOrDefault().Id,
                    Id = t.Id,
                    namepoint = t.PointType.Name,
                    nameclass = t.Subject.Subject_Grade
                        .SelectMany(sg => sg.Grade.Class.Select(c => c.Name))
                        .FirstOrDefault(),
                    Name = t.Name,
                    Code = t.Code.ToString(),
                    SubjectName = t.Subject.Name,
                    Status = t.Status,
                    SubjectId = t.SubjectId,
                    Minute = t.Minute,
                })
                .ToListAsync();

            return testList;
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
                if (maxStudents == null)
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

        [HttpPut("update-test")]
        public async Task<IActionResult> Update_test(TestDTO testDTO)
        {
            var data = await _DbContext.tests.FirstOrDefaultAsync(x => x.Id == testDTO.Id);

            if (data != null)
            {
                data.Name = testDTO.Name;
                data.Minute = testDTO.Minute;
                data.Status = testDTO.Status;
                data.SubjectId=testDTO.SubjectId;
                data.PointTypeId=testDTO.PointTypeId;
                data.MaxStudent = testDTO.Maxstudent;
                _DbContext.Update(data);
                await _DbContext.SaveChangesAsync();
            }

            return Ok("update thành công");
        }

        [HttpDelete("delete-test")]
        public async Task<IActionResult> Delete_test(Guid id)
        {
            var data = await _DbContext.tests.FirstOrDefaultAsync(x => x.Id == id);
            var ListTestCode = _DbContext.testCodes.ToList().FirstOrDefault(x => x.TestId == data.Id);
            var testquestion = _DbContext.testQuestions.ToList().FirstOrDefault(x => x.TestId == data.Id);
            if ( testquestion!=null)
            {
                _DbContext.Remove(testquestion);
                _DbContext.Remove(ListTestCode);
                _DbContext.Remove(data);
              
            }
            else if (ListTestCode != null)
            {
                _DbContext.Remove(ListTestCode);
                _DbContext.Remove(data);
            }
                await _DbContext.SaveChangesAsync();
            return Ok("đã xóa");
        }
    }
}
