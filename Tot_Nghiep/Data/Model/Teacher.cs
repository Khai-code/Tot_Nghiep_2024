using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Teacher
    {
        public Guid Id { get; set; }

        [StringLength(30, ErrorMessage = "Code không được quá 30 ký tự")]
        public string Code { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Class> Class { get; set; }
        public virtual ICollection<Exam_Room> Exam_RoomsAsTeacher1 { get; set; }
        public virtual ICollection<Exam_Room> Exam_RoomsAsTeacher2 { get; set; }
        public virtual ICollection<Teacher_Subject> Teacher_Subject { get; set; }
    }
}
