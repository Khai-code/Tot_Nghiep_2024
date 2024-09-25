using Data.Database;
using Data.DTOs;
using Data.Model;
using Database.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
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
       
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var data= _db.users.FirstOrDefault(temp=>temp.UserName==model.Username);
        
            // Kiểm tra tên người dùng và mật khẩu tại đây (có thể kiểm tra trong cơ sở dữ liệu)
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
                    new Claim(ClaimTypes.Dns,data.Avartar)
                     // Bạn có thể thêm nhiều claim tùy theo nhu cầu
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Thời hạn token
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "https://localhost:7039/",
                    Audience = "https://localhost:7257/"
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Trả về token cho client
                return Ok(new { Token = tokenString });
            }

            // Nếu đăng nhập thất bại
            return Unauthorized();
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
    }
}
