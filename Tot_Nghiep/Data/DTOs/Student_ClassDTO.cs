using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class Student_ClassDTO
    {
        public Guid Id { get; set; }
        public DateTime JoinTime { get; set; }

        [StringLength(500, ErrorMessage = "Độ dài chuỗi ko quá 500 ký tự")]
        public IFormFile StudentProfilePhoto { get; set; }
        public int Status { get; set; }
        public Guid ClassId { get; set; }
        public Guid StudentId { get; set; }
    }
}
