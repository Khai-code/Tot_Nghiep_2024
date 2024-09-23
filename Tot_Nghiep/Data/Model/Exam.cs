using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Exam
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public Guid SubjectId {  get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Exam_Room> Exam_Room { get; set; }
    }
}
