using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class TestQuestionDTO
    {
       

        public Guid Id { get; set; }
        [MaxLength]
        public string QuestionName { get; set; }
        public int Type { get; set; }
        [MaxLength]
        public string RightAnswer { get; set; }
        public Guid TestCodeId { get; set; }
        public List<AnswerDTO> Answers { get; set; }
       
    }
    public class AnswerDTO
    {
        public string Answer { get; set; }
        public Guid Id { get; set; }
    }
}
