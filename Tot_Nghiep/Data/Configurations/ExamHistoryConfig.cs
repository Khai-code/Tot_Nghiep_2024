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
    internal class ExamHistoryConfig : IEntityTypeConfiguration<ExamHistory>
    {
        public void Configure(EntityTypeBuilder<ExamHistory> builder)
        {
            builder.ToTable("ExamHistory");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Exam_Room_Student)
                .WithMany(x => x.ExamHistory)
                .HasForeignKey(x => x.ExamRoomStudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
