using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamHistoriesController : ControllerBase
    {
        AppDbContext _DBContext;
        public ExamHistoriesController(AppDbContext appDbContext)
        {
            _DBContext = appDbContext;  
        }
        [HttpGet("GetAll_ExamHistories")]
        public ActionResult GetALl_ExamHistories()
        {
            return Ok(_DBContext.examHistories.ToList());
        }
        [HttpGet(" GetById_ExamHistories")]
        public ActionResult GetById_ExamHistories(Guid Id)
        {
            return Ok(_DBContext.examHistories.FindAsync(Id));
        }
        [HttpPost("Post_ExamHistories")]
        public async Task<ActionResult> Create_ExamHistories(ExamHistoriesDTO examHistoriesDTO)
        {
            try
            {
                ExamHistory? examHistory = new ExamHistory
                {
                    Id = Guid.NewGuid(),
                    Score = examHistoriesDTO.Score,
                    Note = examHistoriesDTO.Note,
                    CreationTime = examHistoriesDTO.CreationTime,
                    ExamRoomStudentId = examHistoriesDTO.ExamRoomStudentId,
                };
                await _DBContext.examHistories.AddAsync(examHistory);
                await _DBContext.SaveChangesAsync();
                return Ok("Thêm thành Công");
            }catch (Exception)
            {
                return BadRequest("thêm thất bại");
            }
        }
        [HttpPut("UpDate_ExamHistories")]
        public async Task<ActionResult> UpDate_ExamHistories(ExamHistoriesDTO examHistoriesDTO)
        {
            var update= _DBContext.examHistories.FirstOrDefault(temp=>temp.Id==examHistoriesDTO.Id);
            if (update != null)
            {
                update.Score = examHistoriesDTO.Score;
                update.Note= examHistoriesDTO.Note;
                update.CreationTime = examHistoriesDTO.CreationTime;
                update.ExamRoomStudentId=examHistoriesDTO.ExamRoomStudentId;
                _DBContext.examHistories.Update(update);
                await _DBContext.SaveChangesAsync();
                return Ok("sửa thành công");
            }
            return BadRequest("sửa thất bại");
        }
        [HttpDelete("Delete_ExamHistories")]
        public async Task<ActionResult> Delete_ExamHistories(Guid Id)
        {
            var delete= _DBContext.examHistories.FirstOrDefault(temp=> temp.Id==Id);
            if(delete != null)
            {
                _DBContext.examHistories.Remove(delete);
                await _DBContext.SaveChangesAsync();
                return Ok("xóa thành công");
            }
            return BadRequest("xóa thất bại");
        }
    }
}
