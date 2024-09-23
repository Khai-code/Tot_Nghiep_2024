using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Student_Class
    {
        public Guid Id { get; set; }
        public DateTime JoinTime { get; set; }

        [StringLength(500, ErrorMessage = "Độ dài chuỗi ko quá 500 ký tự")]
        public string StudentProfilePhoto { get; set; }
        public int Status { get; set; }
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
        public virtual Class? Class { get; set; }
        public virtual Student? Student { get; set; }
    }
}
