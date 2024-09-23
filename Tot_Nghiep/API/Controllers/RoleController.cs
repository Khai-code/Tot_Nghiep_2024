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
    public class RoleController : ControllerBase
    {
        AppDbContext _db;
        public RoleController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-role")]
        public async Task<ActionResult<List<RoleDTO>>> GetAll()
        {
            try
            {
                var data = await _db.roles.ToListAsync();//.Include(a => a.User)

                if (data == null)
                {
                    return BadRequest("Danh sach chong");
                }
                var RoleDto = data.Select(s => new RoleDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = s.Status,
                }).ToList();

                return Ok(RoleDto);

            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpGet("get-by-id-role")]
        public async Task<ActionResult<RoleDTO>> GetById(Guid Id)
        {
            try
            {
                var data = await _db.roles.FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return BadRequest("Ko co Id nay");
                }

                var roledto = new RoleDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    Status = data.Status,
                };

                return Ok(roledto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> Create(RoleDTO roleDTO)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = roleDTO.Name,
                    Status = roleDTO.Status
                };
                await _db.roles.AddAsync(role);
                await _db.SaveChangesAsync();

                return Ok("Thêm thành công");
            }
            catch (Exception)
            {
                return BadRequest("Lỗi");
            }
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> Update(RoleDTO roleDTO)
        {
            var data = await _db.roles.FirstOrDefaultAsync(x => x.Id == roleDTO.Id);

            if (data != null)
            {
                data.Name = roleDTO.Name;
                data.Status = roleDTO.Status;
                _db.roles.Update(data);
                await _db.SaveChangesAsync();

                return Ok("Update thành công");
            }

            return BadRequest("Ko co Id Nay");
        }

        [HttpDelete("delete-role")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _db.roles.FirstOrDefaultAsync(x => x.Id == Id);
            if (data != null)
            {
                _db.roles.Remove(data);
                await _db.SaveChangesAsync();

                return Ok("Xóa thành công");
            }
            return BadRequest("Lỗi");
        }
    }
}
