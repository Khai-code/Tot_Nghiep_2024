using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class TestDTO
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public Guid SubjectId { get; set; }
    
       
    }
}
