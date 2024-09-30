using System.IO;
using System.Threading.Tasks;
using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Student_ClassController : ControllerBase
    {
        private readonly AppDbContext _db;

        public Student_ClassController(AppDbContext db)
        {
            _db = db;
        }

        //[HttpPost("create-student-class")]
        //public async Task<IActionResult> Create(Student_ClassDTO dto)
        //{
        //    try
        //    {
        //        // Kiểm tra xem ảnh có hợp lệ không
        //        if (dto.StudentProfilePhoto == null || dto.StudentProfilePhoto.Length == 0)
        //        {
        //            return BadRequest("Vui lòng cung cấp ảnh cho học sinh.");
        //        }

        //        // Lưu ảnh tạm thời
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //        Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu không tồn tại

        //        var filePath = Path.Combine(uploadsFolder, dto.StudentProfilePhoto.FileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await dto.StudentProfilePhoto.CopyToAsync(stream);
        //        }

        //        // Kiểm tra xem tệp hình ảnh có tồn tại không


        //        // Khởi tạo FaceRecognition với đường dẫn đến thư mục chứa mô hình
        //        var modelDirectory = Path.Combine(Directory.GetCurrentDirectory(), "models");
        //        using (var faceRecognition = FaceRecognition.Create(modelDirectory))
        //        {
        //            // Tải ảnh và trích xuất khuôn mặt
        //            try
        //            {
        //                using (var image = faceRecognition.LoadImage(filePath))
        //                {
        //                    var faceEncodings = faceRecognition.FaceEncodings(image);

        //                    if (faceEncodings.Count == 0)
        //                    {
        //                        return BadRequest("Không tìm thấy khuôn mặt trong ảnh.");
        //                    }

        //                    // Lưu trữ đặc trưng khuôn mặt
        //                    var faceEncoding = faceEncodings[0]; // Lấy đặc trưng khuôn mặt đầu tiên
        //                    var faceEncodingBase64 = Convert.ToBase64String(faceEncoding.ToArray()); // Chuyển đổi thành base64

        //                    // Tạo đối tượng Student_Class
        //                    var studentClass = new Student_Class
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        StudentId = dto.Id,
        //                        ClassId = dto.ClassId,
        //                        Status = dto.Status,
        //                        JoinTime = DateTime.UtcNow,
        //                        StudentProfilePhoto = $"/uploads/{dto.StudentProfilePhoto.FileName}", // Lưu đường dẫn đến ảnh
        //                        FaceEncoding = faceEncodingBase64 // Lưu trữ đặc trưng khuôn mặt dưới dạng base64
        //                    };

        //                    // Thêm vào cơ sở dữ liệu
        //                    _db.student_Classes.Add(studentClass);
        //                    await _db.SaveChangesAsync();

        //                    return Ok("Thêm học sinh vào lớp thành công");
        //                }
        //            }
        //            catch (Exception imgEx)
        //            {
        //                return BadRequest($"Lỗi khi xử lý hình ảnh: {imgEx.Message}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception if necessary
        //        return BadRequest($"Đã xảy ra lỗi: {ex.Message}");
        //    }
        //}



    }
}
