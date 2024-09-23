using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TestQuestionAnswer
    {
        public Guid Id { get; set; }

        [MaxLength]
        public string Answer { get; set; }
        public Guid TestQuestionId { get; set; }
        public virtual TestQuestion? TestQuestion { get; set; }
        public virtual ICollection<Exam_Room_Student_AnswerHistory>? Exam_Room_Student_AnswerHistories { get; set; }
    }
}
