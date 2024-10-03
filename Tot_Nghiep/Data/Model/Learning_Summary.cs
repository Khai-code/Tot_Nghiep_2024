using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Learning_Summary
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public double Attendance { get; set; }
        public double Point_15 { get; set; }
        public double Point_45 { get; set; }
        public double Point_Midterm { get; set; }
        public double Point_Final { get; set; }
        public double Point_Summary { get; set; }
        public bool IsView { get; set; } = false;
        public Guid SemesterID { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual Student? Student { get;set; }
        public virtual Semester? Semester { get; set; }
    }
}
