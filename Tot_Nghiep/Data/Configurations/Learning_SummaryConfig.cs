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
    public class Learning_SummaryConfig : IEntityTypeConfiguration<Learning_Summary>
    {
        public void Configure(EntityTypeBuilder<Learning_Summary> builder)
        {
            builder.ToTable("Learning_Summary");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Learning_Summaries)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Learning_Summaries)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
