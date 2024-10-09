using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class StudentExamResultDTO
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public string TestCode { get; set; }
        public string RoomName { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public double Score { get; set; }
        public DateTime ExamTime { get; set; }
    }
}
