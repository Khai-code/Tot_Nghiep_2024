using System.ComponentModel.DataAnnotations;

namespace Data.DTOs
{
    public class RoomDTO
    {
        public Guid? Id { get; set; }

        [MaxLength(50, ErrorMessage = "Name ko quá 30 ký tự")]
        public string Name { get; set; }

        [MaxLength(30, ErrorMessage = "Code ko quá 30 ký tự")]
        public string Code { get; set; }
        public int Status { get; set; }
    }
}
