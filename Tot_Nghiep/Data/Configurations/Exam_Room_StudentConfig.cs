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
    internal class Exam_Room_StudentConfig : IEntityTypeConfiguration<Exam_Room_Student>
    {
        public void Configure(EntityTypeBuilder<Exam_Room_Student> builder)
        {
            builder.ToTable("Exam_Room_Student");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Exam_Room_Student)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Exam_Room_TestCode)
                .WithMany(x => x.Exam_Room_Students)
                .HasForeignKey(x => x.ExamRoomTestCodeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
