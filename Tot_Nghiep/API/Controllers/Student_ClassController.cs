using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Data.Database;
using Data.DTOs;
using Data.Model;
using Emgu.CV.Structure;
using Emgu.CV;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Emgu.CV.Face;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Student_ClassController : ControllerBase
    {
        private readonly AppDbContext _db;
        private static VideoCapture _capture;
        private static CascadeClassifier _faceCascade;
        private static LBPHFaceRecognizer _faceRecognizer;
        private static bool _isCapturing = false;

        public Student_ClassController(AppDbContext db)
        {
            _db = db;
            _faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

            // Mở camera (ID 0 - camera mặc định)
            _capture = new VideoCapture(0);
            if (!_capture.IsOpened)
            {
                throw new System.Exception("Unable to open camera.");
            }

            // Tạo một recognizer (để huấn luyện nhận diện khuôn mặt)
            _faceRecognizer = new LBPHFaceRecognizer();
        }
        [HttpPost("clear-face-data")]
        public IActionResult ClearFaceData()
        {
            // Check if the face_model.xml file exists, and delete it
            if (System.IO.File.Exists("face_model.xml"))
            {
                System.IO.File.Delete("face_model.xml");
            }

            return Ok("Face recognition data cleared successfully.");
        }
        private Dictionary<int, Guid> _labelToGuidMapping = new Dictionary<int, Guid>();

        [HttpPost("register-face")]
        public async Task<IActionResult> RegisterFace([FromBody] Student_ClassDTO studentFaceDto)
        {
            // Kiểm tra nếu DTO hợp lệ
            if (studentFaceDto == null || studentFaceDto.StudentId == Guid.Empty)
            {
                return BadRequest("Thông tin sinh viên không hợp lệ.");
            }

            using var frame = _capture.QueryFrame().ToImage<Bgr, byte>();
            var faces = _faceCascade.DetectMultiScale(frame, 1.1, 10, new Size(50, 50));

            if (faces.Length == 0)
                return BadRequest("Không phát hiện khuôn mặt.");

            int currentLabel = _labelToGuidMapping.Count + 1; // Tạo nhãn mới cho mỗi khuôn mặt

            // Huấn luyện recognizer bằng cách sử dụng khuôn mặt đầu vào
            foreach (var face in faces)
            {
                var grayFace = frame.Convert<Gray, byte>().GetSubRect(face).Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic);

                // Huấn luyện mô hình với nhãn mới
                _faceRecognizer.Train(new[] { grayFace.Mat }, new[] { currentLabel });

                // Lưu ánh xạ từ nhãn đến GUID
                _labelToGuidMapping[currentLabel] = studentFaceDto.StudentId;
            }

            // Lưu mô hình sau khi huấn luyện
            _faceRecognizer.Write(studentFaceDto.FaceEncoding);

            // Tạo đối tượng StudentClass từ DTO và lưu vào CSDL
            var studentClass = new Student_Class
            {
                Id = Guid.NewGuid(),
                JoinTime = studentFaceDto.JoinTime,
                StudentProfilePhoto = studentFaceDto.StudentProfilePhoto,
                Status = studentFaceDto.Status,
                ClassId = studentFaceDto.ClassId,
                StudentId = studentFaceDto.StudentId,
                FaceEncoding = studentFaceDto.FaceEncoding,
            };

            await _db.student_Classes.AddAsync(studentClass);
            await _db.SaveChangesAsync(); // Lưu thay đổi

            return Ok("Khuôn mặt đã được đăng ký và mô hình đã lưu thành công.");
        }


        //[HttpPost("register-face")]
        //public IActionResult RegisterFace()
        //{
        //    using var frame = _capture.QueryFrame().ToImage<Bgr, byte>();
        //    var faces = _faceCascade.DetectMultiScale(frame, 1.1, 10, new Size(50, 50));

        //    if (faces.Length == 0)
        //        return BadRequest("No face detected.");
        //    foreach (var face in faces)
        //    {
        //        var grayFace = frame.Convert<Gray, byte>().GetSubRect(face).Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic);
        //        _faceRecognizer.Train(new[] { grayFace.Mat }, new[] { 1 });
        //    }
        //    _faceRecognizer.Write("face_model.xml");

        //    return Ok("lưu thành công");
        //}
        [HttpPost("recognize-face")]
        public IActionResult RecognizeFace()
        {
            // Tải mô hình đã lưu
            if (System.IO.File.Exists("face_model.xml"))
            {
                _faceRecognizer.Read("face_model.xml");
            }
            else
            {
                return StatusCode(500, "Model not trained or saved yet.");
            }

            using var frame = _capture.QueryFrame().ToImage<Bgr, byte>();
            var faces = _faceCascade.DetectMultiScale(frame, 1.1, 10, new Size(50, 50));

            if (faces.Length == 0)
                return BadRequest("No face detected.");

            foreach (var face in faces)
            {
                var grayFace = frame.Convert<Gray, byte>().GetSubRect(face).Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic);
                var result = _faceRecognizer.Predict(grayFace);

                // Kiểm tra nếu khuôn mặt trùng khớp
                if (result.Distance < 100) // Điều chỉnh khoảng cách này nếu cần
                {
                    // Chuyển đổi label từ số nguyên thành GUID
                    var recognizedId = ConvertIntToGuid(result.Label);
                    return Ok($"Face recognized with ID: {recognizedId}");
                }
            }

            return Unauthorized("Face not recognized.");
        }

        // Phương thức chuyển đổi int thành Guid
        private Guid ConvertIntToGuid(int studentIdAsInt)
        {
            // Bạn cần một phương thức phù hợp để ánh xạ lại từ int về GUID
            // Lưu ý: Phương thức này chỉ là một ví dụ, bạn cần thực hiện ánh xạ chính xác
            // Có thể sử dụng một Dictionary hoặc cách lưu trữ khác để giữ ánh xạ giữa int và GUID
            return new Guid(); // Thay thế bằng logic ánh xạ của bạn
        }



        //[HttpGet("recognize-face")]
        //public IActionResult RecognizeFace()
        //{
        //    // Tải mô hình đã lưu
        //    if (System.IO.File.Exists("face_model.xml"))
        //    {
        //        _faceRecognizer.Read("face_model.xml");
        //    }
        //    else
        //    {
        //        return StatusCode(500, "Model not trained or saved yet.");
        //    }

        //    using var frame = _capture.QueryFrame().ToImage<Bgr, byte>();
        //    var faces = _faceCascade.DetectMultiScale(frame, 1.1, 10, new Size(50, 50));

        //    if (faces.Length == 0)
        //        return BadRequest("No face detected.");

        //    foreach (var face in faces)
        //    {
        //        var grayFace = frame.Convert<Gray, byte>().GetSubRect(face).Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic);
        //        var result = _faceRecognizer.Predict(grayFace);

        //        // Kiểm tra nếu khuôn mặt trùng khớp
        //        if (result.Label == 1 && result.Distance < 100)
        //        {
        //            return Ok("Face recognized.");
        //        }
        //    }

        //    return Unauthorized("Face not recognized.");
        //}

        [HttpGet("capture")]
        public IActionResult CaptureFrame()
        {
            try
            {
                // Đọc khung hình từ camera
                using var frame = _capture.QueryFrame().ToImage<Bgr, byte>();
                if (frame == null)
                {
                    return StatusCode(500, "Unable to capture frame.");
                }

                // Phát hiện khuôn mặt trong khung hình
                var faces = _faceCascade.DetectMultiScale(frame, 1.1, 10, new Size(50, 50));

                // Vẽ hình chữ nhật quanh khuôn mặt phát hiện được
                foreach (var face in faces)
                {
                    frame.Draw(face, new Bgr(Color.Red), 2);
                }

                // Chuyển khung hình thành luồng byte để trả về
                using var ms = new MemoryStream();
                frame.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Seek(0, SeekOrigin.Begin);
                var base64Image = Convert.ToBase64String(ms.ToArray());
                // Trả về khung hình dưới dạng ảnh JPEG
                return File(ms.ToArray(), "image/jpeg ");

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("detect")]
        public IActionResult DetectFace(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Invalid image file.");
            }

            try
            {
                // Đọc hình ảnh từ stream
                using var stream = imageFile.OpenReadStream();
                using var img = Image.FromStream(stream);
                using var bitmap = new Bitmap(img);
                using var emguImage = bitmap.ToImage<Bgr, byte>();

                // Tải cascade classifier (được dùng để nhận diện khuôn mặt)
                var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

                // Phát hiện khuôn mặt
                var faces = faceCascade.DetectMultiScale(emguImage, 1.1, 10, new Size(50, 50));

                // Phát hiện các góc nhìn, pose của khuôn mặt nếu cần thiết

                // Vẽ hình chữ nhật xung quanh các khuôn mặt được phát hiện
                foreach (var face in faces)
                {
                    emguImage.Draw(face, new Bgr(Color.Red), 2);

                    // Bạn có thể thêm logic để tính toán góc hoặc phân tích pose tại đây.
                }

                // Lưu hình ảnh đã được xử lý vào bộ nhớ
                using var ms = new MemoryStream();
                emguImage.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var base64Image = Convert.ToBase64String(ms.ToArray());

                return Ok(new { Image = base64Image });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


    }

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
    //        Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu không tồn    tại

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





