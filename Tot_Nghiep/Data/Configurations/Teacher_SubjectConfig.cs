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
    internal class Teacher_SubjectConfig : IEntityTypeConfiguration<Teacher_Subject>
    {
        public void Configure(EntityTypeBuilder<Teacher_Subject> builder)
        {
            builder.ToTable("Teacher_Subject");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Teacher_Subject)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.Teacher_Subject)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
