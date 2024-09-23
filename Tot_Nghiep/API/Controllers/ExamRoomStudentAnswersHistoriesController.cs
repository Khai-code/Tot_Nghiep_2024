using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamRoomStudentAnswersHistoriesController : ControllerBase
    {
        AppDbContext _Dbcontext;
        public ExamRoomStudentAnswersHistoriesController(AppDbContext appDbContext)
        {
            _Dbcontext = appDbContext;
        }
        [HttpGet("GetAll_ExamRoomStudentAnswersHistories")]
        public ActionResult GetAll()
        {
            return Ok(_Dbcontext.exam_Room_Student_AnswerHistories.ToList());
        }
        [HttpGet("GetById_ExamRoomStudentAnswersHistories")]
        public ActionResult GetById(Guid Id)
        {
            return Ok(_Dbcontext.exam_Room_Student_AnswerHistories.FindAsync(Id));
        }
        [HttpPut("Update_ExamRoomStudentAnswersHistories")]
        public async Task<ActionResult> update(ExamRoomStudentAnswersHistoriesDTO dto)
        {
            var update= _Dbcontext.exam_Room_Student_AnswerHistories.FirstOrDefault(temp=>temp.Id==dto.Id);
            if (update!=null)
            {
                update.ExamRoomStudentId= dto.ExamRoomStudentId;
                update.TestQuestionAnswerId= dto.TestQuestionAnswerId;
                _Dbcontext.exam_Room_Student_AnswerHistories.Update(update);
                await _Dbcontext.SaveChangesAsync();
                return Ok("sửa thành công");
            }
            return BadRequest("sửa thất bại");
        }
        [HttpDelete("Delete_ExamRoomStudentAnswersHistories")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var delete = _Dbcontext.exam_Room_Student_AnswerHistories.FirstOrDefault(temp => temp.Id==Id);
            if (delete != null)
            {
                _Dbcontext.exam_Room_Student_AnswerHistories.Remove(delete);
                await _Dbcontext.SaveChangesAsync();
                return Ok("xóa thành công");
            }
            return BadRequest("xóa thất bại");
        }
        [HttpPost("Post_ExamRoomStudentAnswersHistories")]
        public async Task<ActionResult> create(ExamRoomStudentAnswersHistoriesDTO examRoomStudentAnswersHistoriesDTO)
        {
            try
            {
                Exam_Room_Student_AnswerHistory exam_Room_Student_AnswerHistory = new Exam_Room_Student_AnswerHistory
                {
                    Id = Guid.NewGuid(),
                    ExamRoomStudentId = examRoomStudentAnswersHistoriesDTO.ExamRoomStudentId,
                    TestQuestionAnswerId = examRoomStudentAnswersHistoriesDTO.TestQuestionAnswerId,
                };
                _Dbcontext.exam_Room_Student_AnswerHistories.AddAsync(exam_Room_Student_AnswerHistory);
                await _Dbcontext.SaveChangesAsync();
                return Ok("thêm Thành công");
            }catch (Exception)
            {
                return BadRequest("thêm thất bại");
            }
        }
    }
}
