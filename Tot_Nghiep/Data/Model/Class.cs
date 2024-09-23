using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Class
    {
        public Guid Id { get; set; }

        [StringLength(30, ErrorMessage = "Code không được quá 30 ký tự")]
        public string Code { get; set; }

        [StringLength(20, ErrorMessage = "Tên không được quá 20 ký tự")]
        public string Name { get; set; }
        public int Status { get; set; }
        public int MaxStudent { get; set; }
        public Guid TeacherId { get; set; }
        public Guid GradeId { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Grade? Grade { get; set; }
        public virtual ICollection<Notification_Class>? Notification_Classe { get; set; }
        public virtual ICollection<Student_Class>? Student_Classes { get; set; }
    }
}
