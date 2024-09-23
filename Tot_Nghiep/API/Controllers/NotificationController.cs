using Data.Database;
using Data.Model;
using Database.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _db;
        public NotificationController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet("get-all-notification")]
        public async Task<ActionResult<List<NotificationDTO>>> GetAll()
        {
            var data = await _db.notifications.ToListAsync();
            var notificationDto = data.Select(x => new NotificationDTO
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreationTime = x.CreationTime,
                Status = x.Status,
                type = x.type,
            }).ToList();

            return Ok(notificationDto);
        }


        [HttpGet("get-by-id-notification")]
        public async Task<ActionResult<NotificationDTO>> GetByID(Guid Id)
        {
            var data = await _db.notifications.FirstOrDefaultAsync(x => x.Id == Id);

            if (data == null)
            {
                return BadRequest();
            }

            var notificationdto = new NotificationDTO
            {
                Id = data.Id,
                Title = data.Title,
                Content = data.Content,
                CreationTime = data.CreationTime,
                Status = data.Status,
                type = data.type,
            };

            return Ok(notificationdto);
        }


        [HttpPost("create-notification")]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            try
            {
                var noti = new Notification
                {
                    Id = Guid.NewGuid(),
                    Title = notification.Title,
                    Content = notification.Content,
                    CreationTime = DateTime.UtcNow,
                    Status = notification.Status,
                    type = notification.type,
                };
                await _db.notifications.AddAsync(noti);
                await _db.SaveChangesAsync();

                if (noti.type == 1)
                {
                    var validClasses = _db.classes
                                       .Where(c => notification.ClassIds.Contains(c.Id))
                                       .Select(c => c.Id)
                                       .ToList();

                    if (validClasses.Any())
                    {
                        var notificationClasses = validClasses.Select(classId => new Notification_Class
                        {
                            Id = Guid.NewGuid(),
                            NotificationId = noti.Id,
                            ClassId = classId,
                            Status = 1
                        }).ToList();

                        _db.notification_Classes.AddRange(notificationClasses);
                        await _db.SaveChangesAsync();
                    }
                }

                return Ok("Thêm thành công");
            }
            catch (Exception)
            {

                return BadRequest("Lỗi");
            }
        }

        [HttpPut("update-notification")]
        public async Task<IActionResult> Update(NotificationDTO notificationDTO)
        {
            var data = await _db.notifications.FirstOrDefaultAsync(x => x.Id == notificationDTO.Id);
            if (data != null)
            {
                data.Title = notificationDTO.Title;
                data.Content = notificationDTO.Content;
                data.CreationTime = notificationDTO.CreationTime;
                data.Status = notificationDTO.Status;
                data.type = notificationDTO.type;
                _db.notifications.Update(data);
                await _db.SaveChangesAsync();
                return Ok("Update thành công");
            }
            return BadRequest("Lỗi");
        }


        [HttpDelete("delete-notification")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _db.notifications.FirstOrDefaultAsync(x => x.Id == Id);
            if (data != null)
            {
                _db.notifications.Remove(data);
                await _db.SaveChangesAsync();
                return Ok("Xóa thành công");
            }
            return BadRequest("Lỗi");
        }
    }
}
