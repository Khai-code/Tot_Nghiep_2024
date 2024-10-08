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
    public class PointTypeController : ControllerBase
    {
        AppDbContext _db;
        public PointTypeController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAll_pointtype")]
        public async Task<ActionResult<List<PointTypeDTO>>> GetAll()
        {
            var data = await _db.pointTypes.ToListAsync();

            if (data == null)
            {
                return BadRequest("Danh sách null");
            }

            var Point = data.Select(x => new PointTypeDTO 
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return Ok(Point);   
        }


        [HttpGet("get-by-id-poittype")]
        public async Task<ActionResult<PointTypeDTO>> GetById(Guid id)
        {
            var data = await _db.pointTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                return BadRequest("Không có id này");
            }

            var poit = new PointTypeDTO
            {
                Id = data.Id,
                Name = data.Name,
            };

            return Ok(poit);
        }


        [HttpPost("create-poittype")]
        public async Task<IActionResult> Create(PointTypeDTO pointTypeDTO)
        {
            var data = new PointType
            {
                Id = Guid.NewGuid(),
                Name = pointTypeDTO.Name,
            };

            await _db.AddAsync(data);
            await _db.SaveChangesAsync();
            
            return Ok("Thêm thành công");
        }

        [HttpPut("update-poittype")]
        public async Task<IActionResult> Update(PointTypeDTO pointTypeDTO)
        {
            var data = await _db.pointTypes.FirstOrDefaultAsync(x => x.Id == pointTypeDTO.Id);

            if (data == null)
            {
                return BadRequest("Không có Id này");
            }

            data.Name = pointTypeDTO.Name;

            _db.pointTypes.Update(data);
            await _db.SaveChangesAsync();

            return Ok("Update poit thành công");
        }

        [HttpDelete("delete-poitype")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _db.pointTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                return BadRequest("Không có Id này");
            }

            _db.Remove(data);
            await _db.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}
