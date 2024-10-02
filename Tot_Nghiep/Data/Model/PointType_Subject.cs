using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class PointType_Subject
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PointTypeId { get; set; }
        public int Quantity { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual PointType? PointType { get; set; }
        
    }
}
