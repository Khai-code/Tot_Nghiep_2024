using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ClassDTO
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
        //public List<Guid> NotificationIds { get; set; }
    }
}
