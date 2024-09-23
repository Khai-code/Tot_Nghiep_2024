using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Exam_Room_Student
    {
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "CheckinImage không quá 500 ký tự")]
        public string CheckinImage { get; set; }
        public DateTime ChenkTime { get; set; }
        public int Status { get; set; }
        public Guid ExamRoomTestCodeId { get; set; }
        public Guid StudentId { get; set; }
        public virtual Exam_Room_TestCode? Exam_Room_TestCode { get; set; }
        public virtual Student? Student { get; set; }
        public virtual ICollection<ExamHistory>? ExamHistory { get; set; }
        public virtual ICollection<Exam_Room_Student_AnswerHistory>? Exam_Room_Student_AnswerHistory { get; set; }
    }
}
