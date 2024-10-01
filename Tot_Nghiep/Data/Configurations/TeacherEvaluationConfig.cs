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
    internal class TeacherEvaluationConfig : IEntityTypeConfiguration<TeacherEvaluation>
    {

        public void Configure(EntityTypeBuilder<TeacherEvaluation> builder)
        {
            //builder.ToTable("TeacherEvaluation");
            //builder.HasKey(e => e.Id);

            //builder.HasOne<Teacher>()
            //    .WithMany()
            //    .HasForeignKey(e => e.TeacherId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne<Student>()
            //    .WithMany()
            //    .HasForeignKey(e => e.StudentId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
