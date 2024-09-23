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

        [HttpPut("update-class")]
        public async Task<IActionResult> Update(ClassDTO classDTO)
        {
            var data = await _db.classes.FirstOrDefaultAsync(x => x.Id == classDTO.Id);

            if (data == null)
            {
                return NotFound("Ko co lop nay");
            }

            var grade = await _db.classes.FirstOrDefaultAsync(x => x.GradeId == classDTO.GradeId);

            if (grade == null)
            {
                return BadRequest("Ko co Khôi Nay");
            }

            string ClassesName = $"{grade.Name}{classDTO.Name}";

            data.Name = ClassesName;
            data.MaxStudent = classDTO.MaxStudent;
            data.Status = classDTO.Status;
            data.TeacherId = classDTO.TeacherId;
            data.GradeId = classDTO.GradeId;

            _db.classes.Update(data);
            await _db.SaveChangesAsync();

            return Ok("Update thành công");
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
    }
}
