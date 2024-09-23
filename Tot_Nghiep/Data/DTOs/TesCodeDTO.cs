using Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class TesCodeDTO
    {
        public Guid Id { get; set; }

        [StringLength(30, ErrorMessage = "Code ko quá 30 ký tự")]
        public string Code { get; set; }
        public int Status { get; set; }
        public Guid TestId { get; set; }
       
    }
}
