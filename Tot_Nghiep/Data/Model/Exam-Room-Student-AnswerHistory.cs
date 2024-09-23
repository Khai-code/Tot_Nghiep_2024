using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Exam_Room_Student_AnswerHistory
    {
        public Guid Id { get; set; }
        public Guid ExamRoomStudentId { get; set; }
        public Guid TestQuestionAnswerId { get; set; }
        public virtual Exam_Room_Student? Exam_Room_Student {  get; set; }
        public virtual TestQuestionAnswer? TestQuestionAnswer { get; set; }
    }
}
