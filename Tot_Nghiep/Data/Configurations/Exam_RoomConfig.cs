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
    internal class Exam_RoomConfig : IEntityTypeConfiguration<Exam_Room>
    {
        public void Configure(EntityTypeBuilder<Exam_Room> builder)
        {
            builder.ToTable("Exam_Room");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Room)
                .WithMany(x => x.Exam_Room)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Exam)
                .WithMany(x => x.Exam_Room)
                .HasForeignKey(x => x.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Teacher1)
                .WithMany(x => x.Exam_RoomsAsTeacher1)
                .HasForeignKey(x => x.TeacherId1)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Teacher2)
                .WithMany(x => x.Exam_RoomsAsTeacher2)
                .HasForeignKey(x => x.TeacherId2)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
