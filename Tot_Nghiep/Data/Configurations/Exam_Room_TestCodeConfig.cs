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
    internal class Exam_Room_TestCodeConfig : IEntityTypeConfiguration<Exam_Room_TestCode>
    {
        public void Configure(EntityTypeBuilder<Exam_Room_TestCode> builder)
        {
            builder.ToTable("Exam_Room_TestCode");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.TestCode)
                .WithMany(x => x.Exam_Room_TestCodes)
                .HasForeignKey(x => x.TestCodeId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Exam_Room)
                .WithMany(x => x.Exam_Room_TestCode)
                .HasForeignKey(x => x.ExamRoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
