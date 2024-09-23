using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class SystemConfig
    {
        public Guid Id { get; set; }

        [StringLength(100, ErrorMessage = "Tên không được quá 100 ký tự")]
        public string Name { get; set; }
        public int Type { get; set; }

        [MaxLength]
        public string Value { get; set; }
    }
}
