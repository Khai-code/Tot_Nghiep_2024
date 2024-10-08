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
    internal class TestQuestionConfig : IEntityTypeConfiguration<TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestQuestion> builder)
        {
            builder.ToTable("TestQuestion");
            
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Tests)
                .WithMany(x => x.testQuestions)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.NoAction).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
