using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ExamsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var listExam = await _db.exams.AsQueryable().ToListAsync();
            return Ok(listExam);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ExamDTO examDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Exam exam = new Exam();
            exam.Id = Guid.NewGuid();
            exam.CreationTime = DateTime.Now;
            exam.Status = examDTO.Status;
            exam.SubjectId = examDTO.SubjectId;
            await _db.exams.AddAsync(exam);
            await _db.SaveChangesAsync();
            return Ok(exam);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ExamDTO examDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var checkExamToUpdate = _db.exams.FirstOrDefault(c => c.Id == id);
            if (checkExamToUpdate is null)
            {
                return BadRequest("Bai thi khong ton tai");
            }
            checkExamToUpdate.CreationTime = DateTime.Now;
            checkExamToUpdate.Status = examDTO.Status;
            checkExamToUpdate.SubjectId = examDTO.SubjectId;
            _db.exams.Update(checkExamToUpdate);
            await _db.SaveChangesAsync();
            return Ok(checkExamToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var checkExamToDelete = _db.exams.FirstOrDefault(c => c.Id == id);
            if (checkExamToDelete is null)
            {
                return BadRequest("Bai thi khong ton tai");
            }
            _db.exams.Remove(checkExamToDelete);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
