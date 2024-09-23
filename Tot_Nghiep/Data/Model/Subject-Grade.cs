using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Subject_Grade
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public Guid GradeId { get; set; }
        public Guid SubjectId { get; set; }
        public virtual Grade? Grade { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}
