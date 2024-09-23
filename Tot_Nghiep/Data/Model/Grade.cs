using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Grade
    {
        public Guid Id { get; set; }
        public int Name { get; set; }
        public int Status {  get; set; }
        public virtual ICollection<Class> Class { get; set; }
        public virtual ICollection<Subject_Grade> Subject_Grades { get; set; }
    }
}
