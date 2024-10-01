using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class PointType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Test>? tests { get; set; }
        public virtual ICollection<Learning_Summary>? Learning_Summaries { get; set; }
        public virtual ICollection<PointType_Subject>? PointType_Subjects { get; set; }
        public virtual ICollection<Score>? Scores { get; set; }
    }
}
