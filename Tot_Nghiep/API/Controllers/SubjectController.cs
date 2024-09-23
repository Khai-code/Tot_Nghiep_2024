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
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SubjectController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-subject")]
        public async Task<ActionResult<List<SubjectDTO>>> GetAll()
        {
            try
            {
                var data = await _db.subjects.ToListAsync();

                if (data == null)
                {
                    return NotFound("Danh sach null");
                }

                var subjectdto = data.Select(x => new SubjectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    CreationTime = x.CreationTime,
                    Status = x.Status,
                }).ToList();

                return Ok(subjectdto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpGet("get-by-id-subject")]
        public async Task<ActionResult<SubjectDTO>> GetById(Guid Id)
        {
            try
            {
                var data = await _db.subjects.FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return NotFound("Ko co mon nay");
                }

                var subjectdto = new SubjectDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    Code = data.Code,
                    CreationTime = data.CreationTime,
                    Status = data.Status,
                };

                return Ok(subjectdto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
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
        [HttpPost("create-subject")]
        public async Task<IActionResult> Create(SubjectDTO dto)
        {
            try
            {
                var subj = new Subject
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Code = RandomCode(8),
                    CreationTime = DateTime.UtcNow,
                    Status = dto.Status,
                };

                await _db.subjects.AddAsync(subj);
                await _db.SaveChangesAsync();

                foreach (var gradeId in dto.GradeIds)
                {
                    var subjectGrade = new Subject_Grade
                    {
                        Id = Guid.NewGuid(),
                        Status = 1,
                        GradeId = gradeId,
                        SubjectId = subj.Id
                    };

                    await _db.subjects_Grades.AddAsync(subjectGrade);
                    await _db.SaveChangesAsync();
                }

                return Ok("Them thanh cong");
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpPut("update-subject")]
        public async Task<IActionResult> Update(SubjectDTO subjectDTO)
        {
            var data = await _db.subjects.FirstOrDefaultAsync(x => x.Id == subjectDTO.Id);

            if (data != null)
            {
                data.Name = subjectDTO.Name;
                data.CreationTime = DateTime.UtcNow;
                data.Status = subjectDTO.Status;

                _db.subjects.Update(data);
                await _db.SaveChangesAsync();


                return Ok("Update thanh công");
            }

            return BadRequest("Loi");
        }

        [HttpDelete("delete-subject")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _db.subjects.FirstOrDefaultAsync(x => x.Id == Id);

            if (data != null)
            {
                _db.subjects.Remove(data);
                await _db.SaveChangesAsync();

                return Ok("Xoa thanh cong");
            }

            return BadRequest("Loi");
        }
    }
}
