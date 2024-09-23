using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExamRoomStudentDTO
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "CheckinImage không quá 500 ký tự")]
        public string CheckinImage { get; set; }
        public DateTime ChenkTime { get; set; }
        public int Status { get; set; }
        public Guid ExamRoomTestCodeId { get; set; }
        public Guid StudentId { get; set; }
    }
}
