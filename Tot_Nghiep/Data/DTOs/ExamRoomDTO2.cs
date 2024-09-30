using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExamRoomDTO2
    {
        public Guid? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }  // Thêm RoomName
        public Guid ExamId { get; set; }
        public Guid TeacherId1 { get; set; }
        public string Teacher1Code { get; set; }  // Thêm Teacher1Name
        public Guid TeacherId2 { get; set; }
        public string Teacher2Code { get; set; }  // Thêm Teacher2Name
    }
}
