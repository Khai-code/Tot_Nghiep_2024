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

        //[HttpGet("get-all-score")]
        //public async Task<ActionResult<List<ScoreDTO>>> GetAll()
        //{
        //    var listScore = await _db.scores
        //        .Include(er => er.Students)
        //        .Include(er => er.Subjects)
        //        .Include(er => er.PointTypes)
        //        .Select(er => new ScoreDTO
        //        {
        //            Id = er.Id,
        //            StudentId = er.StudentId,
        //            SubjectId = er.SubjectId,
        //            PointTypeId = er.PointTypeId,
        //            SubjectName = er.Subjects.Name,
        //            PointTypeName = er.PointTypes.Name
        //        }).ToListAsync();

        //    return Ok(listScore);
        //}

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

        [HttpGet("get-all-score")]
        public async Task<ActionResult<List<GetScoreDTO>>> GetAll_Scores()
        {
            var data = await _db.scores.ToListAsync();

            if (data == null)
            {
                return NotFound("Danh sách trống");
            }

            var listScore = data.Select(x => new Score
            {
                Id = x.Id,
                StudentId = x.StudentId,
                SubjectId = x.SubjectId,
                PointTypeId = x.PointTypeId,
                Scores = x.Scores

            }).ToList();

            return Ok(listScore);

        }


        [HttpGet("get_all_score_student")]
        public async Task<ActionResult<List<GetScoreDTO>>> GetAll_Score_Student(Guid id)
        {
            var data = await _db.scores.Where(x => x.StudentId == id).ToListAsync();

            if (data == null)
            {
                return NotFound("Danh sách trống");
            }
            var listScoreStudent = data.Select(x => new Score
            {
                Id = x.Id,
                StudentId = x.StudentId,
                SubjectId = x.SubjectId,
                PointTypeId = x.PointTypeId,
                Scores = x.Scores

            }).ToList();

            return Ok(listScoreStudent);

        }

        [HttpPost("generate-scores")]
        public async Task<IActionResult> GenerateScores([FromBody] List<GetScoreDTO> subjectPointTypeDtos)
        {
            if (subjectPointTypeDtos == null || !subjectPointTypeDtos.Any())
            {
                return BadRequest("Danh sách đầu vào không hợp lệ");
            }

            var scores = new List<Score>();

            foreach (var dto in subjectPointTypeDtos)
            {
                var pointTypeSubject = await _db.pointType_Subjects
                    .FirstOrDefaultAsync(pts => pts.SubjectId == dto.SubjectId && pts.PointTypeId == dto.PointTypeId);

                if (pointTypeSubject == null)
                {
                    return NotFound($"Không tìm thấy PointTypeSubject với SubjectId {dto.SubjectId} và PointTypeId {dto.PointTypeId}");
                }

                for (int i = 0; i < pointTypeSubject.Quantity; i++)
                {
                    var score = new Score
                    {
                        Id = Guid.NewGuid(),
                        StudentId = dto.StudentId,
                        SubjectId = dto.SubjectId,
                        PointTypeId = dto.PointTypeId,
                        Scores = 0, // Hoặc logic tính điểm của bạn
                    };

                    scores.Add(score);
                }
            }

            await _db.scores.AddRangeAsync(scores);
            await _db.SaveChangesAsync();

            return Ok(scores);
        }
    }
}
