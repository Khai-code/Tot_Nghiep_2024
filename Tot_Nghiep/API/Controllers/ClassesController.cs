using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        AppDbContext _db;
        public ClassesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-class")]
        public async Task<ActionResult<List<ClassDTO>>> GetAll()
        {
            try
            {
                var data = await _db.classes.ToListAsync();

                if (data == null)
                {
                    return BadRequest("Danh sach null");
                }

                var classdto = data.Select(x => new ClassDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    MaxStudent = x.MaxStudent,
                    Status = x.Status,
                    TeacherId = x.TeacherId,
                    GradeId = x.GradeId,

                }).ToList();

                return Ok(classdto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpGet("gte-by-id-class")]
        public async Task<ActionResult<ClassDTO>> GetById(Guid Id)
        {
            
            try
            {
                var data = await _db.classes.FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return BadRequest("Ko co Id nay");
                }

                var classdto = new ClassDTO
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    MaxStudent = data.MaxStudent,
                    Status = data.Status,
                    TeacherId = data.TeacherId,
                    GradeId = data.GradeId,
                };

                return Ok(classdto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }
        private string RamdomCode(int length)
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

        

        [HttpPost("create-class")]
        public async Task<IActionResult> Create(ClassDTO classDTO)
        {
            try
            {
                var grade = await _db.grades.FirstOrDefaultAsync(x => x.Id == classDTO.GradeId);

                if (grade == null)
                {
                    return BadRequest("Ko tim thay khoi");
                }

                string ClassesName = $"{grade.Name}{classDTO.Name}";

                var data = new Class
                {
                    Id = Guid.NewGuid(),
                    Code = RamdomCode(8),
                    Name = ClassesName,
                    MaxStudent = classDTO.MaxStudent,
                    Status = classDTO.Status,
                    TeacherId= classDTO.TeacherId,
                    GradeId= classDTO.GradeId,
                };

                await _db.classes.AddAsync(data);
                await _db.SaveChangesAsync();

                return Ok("Them thanh công");

            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpDelete("delete-class")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _db.classes.FirstOrDefaultAsync(x => x.Id == Id);
            
            if (data != null)
            {
                _db.classes.Remove(data);
                await _db.SaveChangesAsync();

                return Ok("Da xoa");
            }

            return BadRequest("Ko co lop nay");
        }

        private string RamdomCode_TestCode(int length)
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
        [HttpPut("update-class-and-testcodes")]
        public async Task<IActionResult> UpdateClassAndTestCodes(ClassDTO classDTO)
        {
            try
            {
                // Kiểm tra nếu ClassCode có tồn tại
                if (string.IsNullOrEmpty(classDTO.Code))
                {
                    return BadRequest("ClassCode không được để trống.");
                }

                // Lấy lớp học dựa trên ClassCode
                var classEntity = _db.classes.FirstOrDefault(c => c.Code == classDTO.Code);

                if (classEntity == null)
                {
                    return NotFound("Không tìm thấy lớp học.");
                }

                // Lấy thông tin Subject_Grade dựa trên GradeId của lớp và SubjectId từ DTO
                var subjectGrade = _db.subjects_Grades
                    .FirstOrDefault(sg => sg.GradeId == classEntity.GradeId && sg.SubjectId == classDTO.SubjectId);

                if (subjectGrade == null)
                {
                    return NotFound("Không tìm thấy Subject_Grade phù hợp với lớp học và môn học.");
                }

                // Cập nhật số lượng MaxStudent mới cho lớp
                classEntity.Name = classDTO.Name;
                classEntity.Status = classDTO.Status;
                classEntity.TeacherId = classDTO.TeacherId;
                classEntity.GradeId = classDTO.GradeId;
                classEntity.MaxStudent = classDTO.MaxStudent;
                _db.classes.Update(classEntity);

                // Lấy bài kiểm tra liên quan
                var testEntity = _db.tests.FirstOrDefault(t => t.SubjectId == classDTO.SubjectId);

                if (testEntity == null)
                {
                    return NotFound("Không tìm thấy bài kiểm tra cho lớp học và môn học.");
                }

                // Cập nhật MaxStudent trong bài kiểm tra
                testEntity.MaxStudent = classDTO.MaxStudent;
                _db.tests.Update(testEntity);

                // Lấy tất cả TestCode của bài kiểm tra
                var ListTestCodes = _db.testCodes.Where(tc => tc.TestId == testEntity.Id).ToList();

                // Nếu số lượng hiện tại ít hơn MaxStudent, thêm mới TestCode
                if (ListTestCodes.Count < classDTO.MaxStudent)
                {
                    int missingTestCodes = classDTO.MaxStudent - ListTestCodes.Count;

                    for (int i = 0; i < missingTestCodes; i++)
                    {
                        var newTestCode = new TestCode
                        {
                            Id = Guid.NewGuid(),
                            Code = RamdomCode_TestCode(8), // Tạo mã ngẫu nhiên
                            Status = 1,
                            TestId = testEntity.Id,
                        };

                        await _db.testCodes.AddAsync(newTestCode);
                    }
                }
                // Nếu số lượng hiện tại nhiều hơn MaxStudent, xóa bớt TestCode
                else if (ListTestCodes.Count > classDTO.MaxStudent)
                {
                    int excessTestCodes = ListTestCodes.Count - classDTO.MaxStudent;

                    var testCodesToRemove = ListTestCodes.TakeLast(excessTestCodes).ToList();
                    _db.testCodes.RemoveRange(testCodesToRemove);
                }

                // Lưu các thay đổi
                await _db.SaveChangesAsync();

                return Ok("Cập nhật lớp học và số lượng mã bài kiểm tra thành công.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest($"Lỗi khi cập nhật: {ex.InnerException.Message}");
                }

                return BadRequest($"Lỗi khi cập nhật: {ex.Message}");
            }
        }


    }
}
