using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Student
    {
        public Guid Id { get; set; }

        [StringLength(30, ErrorMessage = "Code không được quá 30 ký tự")]
        public string Code { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Student_Class>? Student_Class { get; set; }
        public virtual ICollection<Exam_Room_Student>? Exam_Room_Student { get; set; }

    }
}
