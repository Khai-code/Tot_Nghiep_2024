using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TestQuestion
    {
        public Guid Id { get; set; }
        [MaxLength]
        public string QuestionName { get; set; }
        public int Type { get; set; }
        [MaxLength]
        public string RightAnswer { get; set; }
        public Guid TestCodeId { get; set; }
        public virtual TestCode? TestCode { get; set; }
        public virtual ICollection<TestQuestionAnswer>? TestQuestionAnswer { get; set; }
    }
}
