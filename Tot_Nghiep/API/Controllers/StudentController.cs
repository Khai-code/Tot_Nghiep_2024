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
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StudentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-student")]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            var data = await _db.students.ToListAsync();
            return Ok(data);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            var data = await _db.students.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(data);
        }

        [HttpGet("coute-student")]
        public async Task<IActionResult> CouteStudent()
        {
            var coute = await _db.students.CountAsync();
            return Ok(coute);
        }

        [HttpGet("get-all-student2")]
        public async Task<ActionResult<List<StudentDTO>>> GetAllName()
        {
            var listStudent = await _db.students
                .Include(er => er.User)
                .Include(er => er.Student_Class)
                .Select(er => new StudentDTO
                {
                    Id = er.Id,
                    Code = er.Code,
                    UserId = er.UserId,
                    Name = er.User.FullName,
                    Email = er.User.Email,
                    DateOfBirth = (DateTime)er.User.DateOfBirth,
                    PhoneNumber = er.User.PhoneNumber
                })
                .ToListAsync();

            return Ok(listStudent);
        }

        [HttpGet("get-by-studentId")]
        public async Task<ActionResult<StudentDTO>> GetById2(Guid studentId)
        {
            // Tìm kiếm sinh viên theo studentId
            var student = await (from s in _db.students
                                 join u in _db.users on s.UserId equals u.Id
                                 where s.Id == studentId
                                 select new StudentDTO
                                 {
                                     Id = s.Id,
                                     Name = u.FullName, // Lấy tên từ bảng Users
                                     Code = s.Code      // Lấy mã từ bảng Students
                                 }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            return Ok(student);
        }
    }
}
