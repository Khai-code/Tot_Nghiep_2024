using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeacherSubjectController : ControllerBase
	{
		AppDbContext _db;

        public TeacherSubjectController(AppDbContext db)
        {
            _db = db;
        }
		[HttpPost("create-TeacherSubject")]
		public async Task<IActionResult> Create (TeacherSubjectDTO teacherSubjectDTO)
		{
			try
			{
				foreach (var subjectID in teacherSubjectDTO.SubjectIds)
				{
					var teacher_Subject = new Teacher_Subject
					{
						Id = Guid.NewGuid(),
						TeacherId = teacherSubjectDTO.TeacherId,
						SubjectId = subjectID

					};
					await _db.teacher_Subjects.AddAsync(teacher_Subject);
					await _db.SaveChangesAsync();

					
				}
				return Ok("Thêm thành công");
			}
			catch (Exception)
			{
				return BadRequest("Lỗi");
			}
		}
    }
}
