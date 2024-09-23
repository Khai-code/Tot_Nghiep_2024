using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class Exam_Room_Student_AnswerHistoryConfig : IEntityTypeConfiguration<Exam_Room_Student_AnswerHistory>
    {
        public void Configure(EntityTypeBuilder<Exam_Room_Student_AnswerHistory> builder)
        {
            builder.ToTable("Exam_Room_Student_AnswerHistory");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Exam_Room_Student)
                .WithMany(x => x.Exam_Room_Student_AnswerHistory)
                .HasForeignKey(x => x.ExamRoomStudentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.TestQuestionAnswer)
                .WithMany(x => x.Exam_Room_Student_AnswerHistories)
                .HasForeignKey(x => x.TestQuestionAnswerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
