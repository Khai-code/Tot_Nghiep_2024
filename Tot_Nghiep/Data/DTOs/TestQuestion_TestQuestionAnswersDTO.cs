﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class TestQuestion_TestQuestionAnswersDTO
    {
        public string QuestionName { get; set; }
        public int QuestionType { get; set; } // Xác định loại câu hỏi
        public string CreatedByName { get; set; }
        public Guid TestId { get; set; }
        public List<string> Answers { get; set; } // Danh sách các đáp án (nếu có)
        public List<string> CorrectAnswers { get; set; } // Danh sách đáp án đúng (cho câu hỏi nhiều đáp án đúng)
    }
}