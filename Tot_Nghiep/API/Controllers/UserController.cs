using Data.Database;
using Data.DTOs;
using Data.Model;
using Database.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("get-all-user")]
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            var data = await _db.users.ToListAsync();
            var UserDTO = data.Select(x => new UserDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                Avartar = x.Avartar,
                Email = x.Email,
                UserName = x.UserName,
                PasswordHash = x.PasswordHash,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                IsLocked = x.IsLocked,
                LockedEndTime = x.LockedEndTime,
                CreationTime = x.CreationTime,
                LastMordificationTime = x.LastMordificationTime,
                Status = x.Status,
                RoleId = x.RoleId,
            }).ToList();

            return Ok(UserDTO);
        }
        [HttpGet("get-by-id-user")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id)
        {
            var data = await _db.users.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                return BadRequest();
            }
            else
            {
                var UserDTO = new UserDTO
                {
                    Id = data.Id,
                    FullName = data.FullName,
                    Avartar = data.Avartar,
                    Email = data.Email,
                    UserName = data.UserName,
                    PasswordHash = data.PasswordHash,
                    DateOfBirth = data.DateOfBirth,
                    PhoneNumber = data.PhoneNumber,
                    IsLocked = data.IsLocked,
                    LockedEndTime = data.LockedEndTime,
                    CreationTime = data.CreationTime,
                    LastMordificationTime = data.LastMordificationTime,
                    Status = data.Status,
                    RoleId = data.RoleId,

                };

                return Ok(UserDTO);
            }
        }

        private string RandomCode(int length)
        {
            const string CodeNew = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = CodeNew[random.Next(CodeNew.Length)];
            }

            return new string(code);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> Create(UserDTO user)
        {
            try
            {

                var data = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = user.FullName,
                    Avartar = user.Avartar,
                    Email = user.Email,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    IsLocked = user.IsLocked,
                    LockedEndTime = user.LockedEndTime,
                    CreationTime = DateTime.UtcNow,
                    LastMordificationTime = user.LastMordificationTime,
                    Status = user.Status,
                    RoleId = user.RoleId,
                };
                await _db.users.AddAsync(data);
                await _db.SaveChangesAsync();

                // Kiểm tra để tạo Student hoặc Teacher

                var roleid = _db.roles.FirstOrDefault(x => x.Id == data.RoleId);

                if (roleid.Name == "Student")
                {
                    var student = new Student
                    {
                        Id = Guid.NewGuid(),
                        Code = RandomCode(8),
                        UserId = data.Id
                    };

                    await _db.students.AddAsync(student);
                    await _db.SaveChangesAsync();
                }

                if (roleid.Name == "Teacher")
                {
                    var teachr = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        Code = RandomCode(8),
                        UserId = data.Id
                    };

                    await _db.teachers.AddAsync(teachr);
                    await _db.SaveChangesAsync();

                }

                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }
        [HttpPost("validate-test-code")]
        [Authorize] // Xác thực token của người dùng đã đăng nhập
        public IActionResult ValidateTestCode([FromBody] TestCodeRequest request)
        {
            // Lấy thông tin `StudentId` từ token
            var studentIdFromToken = User.FindFirst("StudentId")?.Value;
            if (studentIdFromToken == null)
            {
                return Unauthorized(new { Message = "Invalid token. StudentId not found." });
            }

            // Tìm sinh viên trong bảng `students`
            var student = _db.students.FirstOrDefault(s => s.Id == Guid.Parse(studentIdFromToken));
            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            // Kiểm tra mã bài thi có tồn tại trong bảng `testCodes`
            var testCode = _db.testCodes.FirstOrDefault(tc => tc.Code == request.TestCode);
            if (testCode == null)
            {
                return NotFound(new { Message = "Invalid test code." });
            }

            // Kiểm tra thông tin trong bảng `exam_Room_Students` dựa trên `StudentId` và `TestCode`
            var examRoomTestCode = _db.exam_Room_TestCodes.FirstOrDefault(ertc => ertc.TestCodeId == testCode.Id);
            if (examRoomTestCode == null)
            {
                return NotFound(new { Message = "Test code not associated with any exam room." });
            }

            var examRoomStudent = _db.exam_Room_Students.FirstOrDefault(ers => ers.StudentId == student.Id && ers.ExamRoomTestCodeId == examRoomTestCode.Id);
            if (examRoomStudent == null)
            {
                return Unauthorized(new { Message = "Student not assigned to this exam room or invalid test code." });
            }

            // Xác thực thành công
            return Ok(new { Message = "Test code and student validated successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var data = _db.users.FirstOrDefault(temp => temp.UserName == model.Username);
            var student = _db.roles.FirstOrDefault(temp => temp.Id == data.RoleId);
            if (model.Username == data.UserName && model.Password == data.PasswordHash)
            {
                // Nếu thông tin đăng nhập đúng, tạo token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, data.FullName),
                    new Claim("Id",student.Name.ToString())
                    //new Claim("Id", student != null ? student.Name : "N/A"),
                    //new Claim("Idteacher",teacher != null? teacher.Code:"N/A")
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "https://localhost:7039/",
                    Audience = "https://localhost:7257/"
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Trả về token cho client
                return Ok(new { Token = tokenString });
            }
            return Unauthorized("tên đăng nhập mật khẩu không đúng");
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {

            return Ok(new { message = "Logout successful" });
        }
        [HttpPut("update-user")]
        public async Task<IActionResult> Update(UserDTO userDTO)
        {
            var data = await _db.users.FirstOrDefaultAsync(x => x.Id == userDTO.Id);

            if (data == null)
            {
                return NotFound("Không tìm thấy user");
            }

            data.FullName = userDTO.FullName;
            data.Avartar = userDTO.Avartar;
            data.Email = userDTO.Email;
            data.UserName = userDTO.UserName;
            data.PasswordHash = userDTO.PasswordHash;
            data.DateOfBirth = userDTO.DateOfBirth;
            data.PhoneNumber = userDTO.PhoneNumber;
            data.IsLocked = userDTO.IsLocked;
            data.LockedEndTime = userDTO.LockedEndTime;
            data.CreationTime = userDTO.CreationTime;
            data.LastMordificationTime = DateTime.UtcNow;
            data.Status = userDTO.Status;
            data.RoleId = userDTO.RoleId;

            _db.users.Update(data);
            _db.SaveChanges();

            // Tìm role mới dựa trên rollID
            var newrole = await _db.roles.FirstOrDefaultAsync(x => x.Id == data.RoleId);

            if (newrole.Name == "Student")
            {
                // Kiểm tra và thêm Student nếu chưa tồn tại
                var teacherid = await _db.teachers.FirstOrDefaultAsync(x => x.UserId == data.Id);
                if (teacherid != null)
                {
                    _db.teachers.Remove(teacherid);

                    var student = new Student
                    {
                        Id = Guid.NewGuid(),
                        Code = RandomCode(8),
                        UserId = data.Id
                    };

                    _db.students.Add(student);
                    _db.SaveChanges();
                }
            }

            if (newrole.Name == "Teacher")
            {
                // Kiểm tra và thêm Student nếu chưa tồn tại
                var studentid = await _db.students.FirstOrDefaultAsync(x => x.UserId == data.Id);
                if (studentid != null)
                {
                    _db.students.Remove(studentid);

                    var teachr = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        Code = RandomCode(8),
                        UserId = data.Id
                    };

                    _db.teachers.Add(teachr);
                    _db.SaveChanges();
                }
            }

            return Ok("Update thành công");
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _db.users.FirstOrDefaultAsync(x => x.Id == id);

            var student = await _db.students.FirstOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                _db.students.Remove(student);
            }

            // Xóa thông tin của user trong bảng teachers (nếu có)
            var teacher = await _db.teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher != null)
            {
                _db.teachers.Remove(teacher);
            }

            // Xóa user trong bảng users
            _db.users.Remove(data);
            await _db.SaveChangesAsync();

            return Ok("Xóa thành công");
        }

        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(Guid Id, IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
            {
                return BadRequest("Ảnh không hợp lệ");
            }
            // Tìm user theo ID
            var user = await _db.users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại");
            }
            // Đọc file ảnh và chuyển thành base64 hoặc đường dẫn file
            using (var memoryStream = new MemoryStream())
            {
                await avatarFile.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);

                //Lưu Base64 của ảnh vào trưởng Avatar
                user.Avartar = base64Image;

                // Cập nhật thời gian thay đổi cuối cùng
                user.LastMordificationTime = DateTime.Now;

                // Lưu thay đổi vào db
                await _db.SaveChangesAsync();
            }
            return Ok("Tải ảnh thành công");
        }
    }
}
