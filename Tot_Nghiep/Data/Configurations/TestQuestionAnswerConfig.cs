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
    internal class TestQuestionAnswerConfig : IEntityTypeConfiguration<TestQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<TestQuestionAnswer> builder)
        {
            builder.ToTable("TestQuestionAnswer");
            
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TestQuestion)
                .WithMany(x => x.TestQuestionAnswer)
                .HasForeignKey(x => x.TestQuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
