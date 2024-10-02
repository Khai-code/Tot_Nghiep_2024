using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Score
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PointTypeId { get; set; }
        public double Scores { get; set; }
        public virtual Student? Students { get; set; }
        public virtual Subject? Subjects { get; set; }
        public virtual PointType? PointTypes { get; set; }
    }
}
