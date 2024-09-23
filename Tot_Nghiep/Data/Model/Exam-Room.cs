using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Exam_Room
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public Guid RoomId { get; set; }
        public Guid ExamId { get; set; }
        public Guid TeacherId1 { get; set; }
        public Guid TeacherId2 { get; set; }
        public virtual Room? Room {  get; set; }
        public virtual Exam? Exam { get; set; }

        [ForeignKey("TeacherId1")]
        [InverseProperty("Exam_RoomsAsTeacher1")]
        public virtual Teacher? Teacher1 { get; set; }

        [ForeignKey("TeacherId2")]
        [InverseProperty("Exam_RoomsAsTeacher2")]
        public virtual Teacher? Teacher2 { get; set; }
        public virtual ICollection<Exam_Room_TestCode> Exam_Room_TestCode { get; set; }
    }
}
