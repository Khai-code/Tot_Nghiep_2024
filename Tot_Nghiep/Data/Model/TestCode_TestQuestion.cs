using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TestCode_TestQuestion
    {
        public Guid Id { get; set; }
        public Guid TestCodeId { get; set; }
        public Guid TestQuestionId { get; set; }
        public virtual TestCode? TestCodes { get; set; }
        public virtual TestQuestion? TestQuestion { get; set; }
    }
}
