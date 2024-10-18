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
        public string CreatedByName { get; set; }
        public int Level { get; set; }
        public Guid? TestId { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }

    public class AnswerDTO
    {
        public string Answer { get; set; }
        public Guid Id { get; set; }
    }

    public class DetailDTO
    {
        public Guid IdTestcode { get; set; }
        public string CodeTescode { get; set; }
        public int? time { get; set; }
        public List<TestQuestionDTO> NameQuestion { get; set; } // Chỉnh sửa kiểu dữ liệu
        public string NameSubject { get; set; }
        public string Nameclass { get; set; }
        public string codestudent { get; set; }
    }
}

