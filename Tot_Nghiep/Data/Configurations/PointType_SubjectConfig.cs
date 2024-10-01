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
    public class PointType_SubjectConfig : IEntityTypeConfiguration<PointType_Subject>
    {
        public void Configure(EntityTypeBuilder<PointType_Subject> builder)
        {
            builder.ToTable("PointType_Subject");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.PointType_Subjects)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PointType)
                .WithMany(x => x.PointType_Subjects)
                .HasForeignKey(x => x.PointTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
