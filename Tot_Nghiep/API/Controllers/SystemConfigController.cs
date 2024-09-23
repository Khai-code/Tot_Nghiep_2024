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
    public class SystemConfigController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SystemConfigController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-SystemConfig")]
        public async Task<ActionResult<List<SystemConfigDTO>>> GetAll()
        {
            var data = await _db.systemConfigs.ToListAsync();

            var sysdto = data.Select(x => new SystemConfigDTO
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Value = x.Value,
            }).ToList();

            return Ok(sysdto);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<SystemConfig>> GetById(Guid Id)
        {
            try
            {
                var data = await _db.systemConfigs.FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return BadRequest("Ko co Id nay");
                }

                var sysdto = new SystemConfigDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    Type = data.Type,
                    Value = data.Value,
                };

                return Ok(sysdto);
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpPost("create- system")]
        public async Task<IActionResult> Create(SystemConfigDTO sysdto)
        {
            try
            {
                var sys = new SystemConfig
                {
                    Id = Guid.NewGuid(),
                    Name = sysdto.Name,
                    Type = sysdto.Type,
                    Value = sysdto.Value,
                };

                await _db.systemConfigs.AddAsync(sys);
                await _db.SaveChangesAsync();

                return Ok("Them thanh cong");
            }
            catch (Exception)
            {
                return BadRequest("Loi");
            }
        }

        [HttpPut("update-system")]
        public async Task<IActionResult> Update(SystemConfigDTO sysdto)
        {
            var data = await _db.systemConfigs.FirstOrDefaultAsync(x => x.Id == sysdto.Id);

            if (data != null)
            {
                data.Name = sysdto.Name;
                data.Type = sysdto.Type;
                data.Value = sysdto.Value;
                _db.systemConfigs.Update(data);
                await _db.SaveChangesAsync();

                return Ok("Update thanh cong");
            }

            return BadRequest("Loi");
        }

        [HttpDelete("delete-system")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = _db.systemConfigs.FirstOrDefault(x => x.Id == Id);
            if (data != null)
            {
                _db.systemConfigs.Remove(data);
                await _db.SaveChangesAsync();

                return Ok("Delete thanh cong");
            }

            return BadRequest("Khong co Id nay");
        }
    }
}
