using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExamHistoriesDTO
    {
        public Guid Id { get; set; }
        public double Score { get; set; }

        [MaxLength]
        public string? Note { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid ExamRoomStudentId { get; set; }
    }
}
