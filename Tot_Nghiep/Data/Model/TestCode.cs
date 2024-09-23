using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TestCode
    {
        public Guid Id { get; set; }

        [StringLength(30, ErrorMessage = "Code ko quá 30 ký tự")]
        public string Code { get; set; }
        public int Status { get; set; }
        public Guid TestId { get; set; }
        public virtual Test? Test { get; set; }
        public virtual ICollection<TestQuestion>? TestQuestion { get; set; }
        public virtual ICollection<Exam_Room_TestCode>? Exam_Room_TestCodes { get; set; }
    }
}
