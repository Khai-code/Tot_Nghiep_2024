using Data.Database;
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
    }
}
