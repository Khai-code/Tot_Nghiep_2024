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
    public class ExamRoomTestCodeController : ControllerBase
    {
        AppDbContext _DbContext;
        public ExamRoomTestCodeController(AppDbContext appDb)
        {
            _DbContext = appDb;
        }
        [HttpGet("GetAll_ExamRoomTestCode")]
        public ActionResult GetAll()
        {
            return Ok(_DbContext.exam_Room_TestCodes.ToList());
        }
        [HttpGet("GetById_ExamRoomTestCode")]
        public ActionResult GetById(Guid Id)
        {
            return Ok(_DbContext.exam_Room_TestCodes.FindAsync(Id));
        }
        [HttpPost("Post_ExamRoomTestCode")]
        public async Task<ActionResult> Create(ExamRoomTestCodeDTO examRoomTestCodeDTO)
        {
            try
            {
                // Tạo và lưu Exam_Room_TestCode
                Exam_Room_TestCode exam_Room_TestCode = new Exam_Room_TestCode
                {
                    Id = Guid.NewGuid(),
                    TestCodeId = examRoomTestCodeDTO.TestCodeId,
                    ExamRoomId = examRoomTestCodeDTO.ExamRoomId
                };
                await _DbContext.exam_Room_TestCodes.AddAsync(exam_Room_TestCode);
                await _DbContext.SaveChangesAsync();

                // Tìm kiếm exam_room_student hiện có
                var exam_room_student = _DbContext.exam_Room_Students.FirstOrDefault(temp => temp.Id == examRoomTestCodeDTO.Id);
                // Tạo và lưu Exam_Room_Student
                Exam_Room_Student exam = new Exam_Room_Student
                {
                    Id = Guid.NewGuid(),
                    CheckinImage = exam_room_student.CheckinImage,
                    ChenkTime = exam_room_student.ChenkTime,
                    Status = exam_room_student.Status,
                    ExamRoomTestCodeId = examRoomTestCodeDTO.Id,
                    StudentId = exam_room_student.StudentId,
                };
                await _DbContext.exam_Room_Students.AddAsync(exam);
                await _DbContext.SaveChangesAsync();

                return Ok("Thêm thành công");
            }
            catch (Exception)
            {
                return BadRequest("Thêm thất bại");
            }
        }
        [HttpDelete("Delete_ExamRoomTestCode/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                // Tìm kiếm Exam_Room_TestCode hiện có dựa trên Id
                var existingExamRoomTestCode = await _DbContext.exam_Room_TestCodes
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (existingExamRoomTestCode == null)
                {
                    return NotFound("Không tìm thấy mã phòng thi");
                }

                // Xóa Exam_Room_TestCode
                _DbContext.exam_Room_TestCodes.Remove(existingExamRoomTestCode);

                // Tìm kiếm Exam_Room_Student hiện có dựa trên ExamRoomTestCodeId
                var existingExamRoomStudent = await _DbContext.exam_Room_Students
                    .FirstOrDefaultAsync(s => s.ExamRoomTestCodeId == id);

                if (existingExamRoomStudent != null)
                {
                    // Xóa Exam_Room_Student liên quan
                    _DbContext.exam_Room_Students.Remove(existingExamRoomStudent);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _DbContext.SaveChangesAsync();

                return Ok("Xóa thành công");
            }
            catch (Exception)
            {
                return BadRequest("Xóa thất bại");
            }
        }
        [HttpPut("Put_ExamRoomTestCode")]
        public async Task<ActionResult> Update([FromForm]ExamRoomTestCodeDTO examRoomTestCodeDTO, ExamRoomStudentDTO examRoomStudentDTO)
        {
            try
            {
                // Tìm kiếm Exam_Room_TestCode hiện có dựa trên Id
                var existingExamRoomTestCode = await _DbContext.exam_Room_TestCodes
                    .FirstOrDefaultAsync(e => e.Id == examRoomTestCodeDTO.Id);

                if (existingExamRoomTestCode == null)
                {
                    return NotFound("Không tìm thấy mã phòng thi");
                }

                // Cập nhật thông tin của Exam_Room_TestCode
                existingExamRoomTestCode.TestCodeId = examRoomTestCodeDTO.TestCodeId;
                existingExamRoomTestCode.ExamRoomId = examRoomTestCodeDTO.ExamRoomId;
                _DbContext.exam_Room_TestCodes.Update(existingExamRoomTestCode);

                // Lưu các thay đổi của Exam_Room_TestCode
                await _DbContext.SaveChangesAsync();

                // Tìm kiếm Exam_Room_Student hiện có dựa trên ExamRoomTestCodeId
                var existingExamRoomStudent = await _DbContext.exam_Room_Students
                    .FirstOrDefaultAsync(s => s.ExamRoomTestCodeId == examRoomTestCodeDTO.Id);

                if (existingExamRoomStudent == null)
                {
                    return NotFound("Không tìm thấy học sinh");
                }

                // Cập nhật thông tin của Exam_Room_Student
                existingExamRoomStudent.CheckinImage = examRoomStudentDTO.CheckinImage;
                existingExamRoomStudent.ChenkTime = examRoomStudentDTO.ChenkTime;
                existingExamRoomStudent.Status = examRoomStudentDTO.Status;
                existingExamRoomStudent.StudentId = examRoomStudentDTO.StudentId;

                _DbContext.exam_Room_Students.Update(existingExamRoomStudent);

                // Lưu các thay đổi của Exam_Room_Student
                await _DbContext.SaveChangesAsync();

                return Ok("Cập nhật thành công");
            }
            catch (Exception)
            {
                return BadRequest("Cập nhật thất bại");
            }
        }


    }
}
