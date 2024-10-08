using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    internal class TestQuestionAnswerDTO
    {
        public Guid Id { get; set; }

        [MaxLength]
        public string Answer { get; set; }
        public Guid TestQuestionId { get; set; }
    }
}
