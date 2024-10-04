using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ScoreController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-score")]
        public async Task<ActionResult<List<ScoreDTO>>> GetAll()
        {
            var listScore = await _db.scores
                .Include(er => er.Students)
                .Include(er => er.Subjects)
                .Include(er => er.PointTypes)
                .Select(er => new ScoreDTO
                {
                    Id = er.Id,
                    StudentId = er.StudentId,
                    SubjectId = er.SubjectId,
                    PointTypeId = er.PointTypeId,
                    SubjectName = er.Subjects.Name,
                    PointTypeName = er.PointTypes.Name
                }).ToListAsync();

            return Ok(listScore);
        }

        [HttpPost("create-score")]
        public async Task<IActionResult> Create(ScoreDTO scoreDTO)
        {
            try
            {
                var score = new Score
                {
                    Id = Guid.NewGuid(),
                    StudentId = scoreDTO.StudentId,
                    SubjectId = scoreDTO.SubjectId,
                    PointTypeId = scoreDTO.PointTypeId,
                    Scores = scoreDTO.Scores
                };

                await _db.scores.AddAsync(score);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest("Thêm thất bại");
            }
        }
    }
}
