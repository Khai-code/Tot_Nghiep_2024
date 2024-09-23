using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExamRoomTestCodeDTO
    {
        public Guid Id { get; set; }
        public Guid TestCodeId { get; set; }
        public Guid ExamRoomId { get; set; }
    }
}
