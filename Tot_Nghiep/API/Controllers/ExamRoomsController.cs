using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamRoomsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ExamRoomsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var listExamRoom = await _db.exam_Rooms.AsQueryable().ToListAsync();
            return Ok(listExamRoom);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var ExamRoom = _db.exam_Rooms.FirstOrDefault(c => c.Id == id);
            if (ExamRoom is null)
            {
                return NotFound("Khong tim thay phong thi");
            }
            return Ok(ExamRoom);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ExamRoomDTO examRoomDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (examRoomDTO.StartTime > examRoomDTO.EndTime)
            {
                return BadRequest("Thoi gian bat dau phai nho hon thoi gian ket thuc");
            }
            Exam_Room examRoom = new Exam_Room();
            examRoom.Id = Guid.NewGuid();
            examRoom.StartTime = examRoomDTO.StartTime;
            examRoom.EndTime = examRoomDTO.EndTime;
            examRoom.Status = examRoomDTO.Status;
            examRoom.ExamId = examRoomDTO.ExamId;
            examRoom.RoomId = examRoomDTO.RoomId;
            examRoom.TeacherId1 = examRoomDTO.TeacherId1;
            examRoom.TeacherId2 = examRoomDTO.TeacherId2;
            await _db.exam_Rooms.AddAsync(examRoom);
            await _db.SaveChangesAsync();
            return Ok(examRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ExamRoomDTO examRoomDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var checkExamRoomToUpdate = _db.exam_Rooms.FirstOrDefault(c => c.Id == id);
            if (checkExamRoomToUpdate is null)
            {
                return BadRequest("Phong thi khong ton tai");
            }
            checkExamRoomToUpdate.StartTime = examRoomDTO.StartTime;
            checkExamRoomToUpdate.EndTime = examRoomDTO.EndTime;
            checkExamRoomToUpdate.Status = examRoomDTO.Status;
            checkExamRoomToUpdate.ExamId = examRoomDTO.ExamId;
            checkExamRoomToUpdate.RoomId = examRoomDTO.RoomId;
            checkExamRoomToUpdate.TeacherId1 = examRoomDTO.TeacherId1;
            checkExamRoomToUpdate.TeacherId2 = examRoomDTO.TeacherId2;
            _db.exam_Rooms.Update(checkExamRoomToUpdate);
            await _db.SaveChangesAsync();
            return Ok(checkExamRoomToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var checkExamRoomToDelete = _db.exam_Rooms.FirstOrDefault(c => c.Id == id);
            if (checkExamRoomToDelete is null)
            {
                return BadRequest("Phong khong ton tai");
            }
            _db.exam_Rooms.Remove(checkExamRoomToDelete);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
