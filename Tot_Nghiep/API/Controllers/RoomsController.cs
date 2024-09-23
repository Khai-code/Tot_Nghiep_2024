using Data.Database;
using Data.DTOs;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RoomsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var listRoom = await _db.rooms.AsQueryable().ToListAsync();
            return Ok(listRoom);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var Room = _db.rooms.FirstOrDefault(c => c.Id == id);
            if (Room is null)
            {
                return NotFound("Khong tim thay phong");
            }
            return Ok(Room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var checkRoomCreate = _db.rooms.FirstOrDefault(c => c.Name.ToLower() == roomDTO.Name.ToLower());
            if (checkRoomCreate is not null)
            {
                return BadRequest("Ten phong da ton tai");
            }
            Room room = new Room();
            room.Id = Guid.NewGuid();
            room.Name = roomDTO.Name;
            room.Code = await GenerateCode(8);
            room.Status = roomDTO.Status;
            await _db.rooms.AddAsync(room);
            await _db.SaveChangesAsync();
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] RoomDTO roomDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var checkRoomToUpdate = _db.rooms.FirstOrDefault(c => c.Id == id);
            if (checkRoomToUpdate is null)
            {
                return BadRequest("Phong khong ton tai");
            }
            var checkRoomExist = _db.rooms.FirstOrDefault(c => (c.Name.ToLower() == roomDTO.Name.ToLower() || c.Code == roomDTO.Code) && c.Id != id);
            if (checkRoomExist is not null)
            {
                return BadRequest("Ten phong hoac ma phong da ton tai");
            }
            checkRoomToUpdate.Code = roomDTO.Code;
            checkRoomToUpdate.Status = roomDTO.Status;
            checkRoomToUpdate.Name = roomDTO.Name;
            _db.rooms.Update(checkRoomToUpdate);
            await _db.SaveChangesAsync();
            return Ok(checkRoomToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var checkRoomToDelete = _db.rooms.FirstOrDefault(c => c.Id == id);
            if (checkRoomToDelete is null)
            {
                return BadRequest("Phong khong ton tai");
            }
            _db.rooms.Remove(checkRoomToDelete);
            await _db.SaveChangesAsync();
            return Ok();
        }
        private async Task<string> GenerateCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            var checkCode = await _db.rooms.AnyAsync(c => c.Code == finalString);
            if (checkCode)
            {
                return await GenerateCode(8);

            }
            else
            {
                return finalString;
            }
        }
    }
}
