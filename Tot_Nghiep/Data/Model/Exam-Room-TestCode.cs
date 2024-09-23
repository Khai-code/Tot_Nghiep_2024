using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Exam_Room_TestCode
    {
        public Guid Id { get; set; }
        public Guid TestCodeId { get; set; }
        public Guid ExamRoomId { get; set; }
        public virtual TestCode? TestCode { get; set; }
        public virtual Exam_Room? Exam_Room { get; set; }
        public virtual ICollection<Exam_Room_Student>? Exam_Room_Students { get; set; }
    }
}
