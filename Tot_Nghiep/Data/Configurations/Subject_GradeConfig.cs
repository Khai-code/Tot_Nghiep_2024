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
    internal class Subject_GradeConfig : IEntityTypeConfiguration<Subject_Grade>
    {
        public void Configure(EntityTypeBuilder<Subject_Grade> builder)
        {
            builder.ToTable("Subject_Grade");

            builder.HasKey(x => x.Id);  

            builder.HasOne(x => x.Grade)
                .WithMany(x => x.Subject_Grades)
                .HasForeignKey(x => x.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Subject_Grade)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
