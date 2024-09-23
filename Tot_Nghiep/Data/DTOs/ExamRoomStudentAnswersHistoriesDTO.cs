using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ExamRoomStudentAnswersHistoriesDTO
    {
        public Guid Id { get; set; }
        public Guid ExamRoomStudentId { get; set; }
        public Guid TestQuestionAnswerId { get; set; }
    }
}
