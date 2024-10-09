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
        public int Level { get; set; }
        [MaxLength]
        public string RightAnswer { get; set; }
        public string CreatedByName { get; set; }
        public Guid? TestId { get; set; }
        public virtual Test? Tests { get; set; }
        public virtual ICollection<TestQuestionAnswer>? TestQuestionAnswer { get; set; }
        public virtual ICollection<TestCode_TestQuestion>? TestCode_TestQuestions { get; set; }
    }
}
