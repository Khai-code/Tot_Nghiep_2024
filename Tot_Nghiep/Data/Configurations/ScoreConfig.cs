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
    public class ScoreConfig : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable("Score");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Students)
                .WithMany(x => x.Scores)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Subjects)
                .WithMany(x => x.Scores)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PointTypes)
                .WithMany(x => x.Scores)
                .HasForeignKey(x => x.PointTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
