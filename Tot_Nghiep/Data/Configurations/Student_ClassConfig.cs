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
    internal class Student_ClassConfig : IEntityTypeConfiguration<Student_Class>
    {
        public void Configure(EntityTypeBuilder<Student_Class> builder)
        {
            builder.ToTable("Student_Class");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Student_Class)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Class)
                .WithMany(x => x.Student_Classes)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
