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
    internal class TestCode_TestQuestionConfig : IEntityTypeConfiguration<TestCode_TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestCode_TestQuestion> builder)
        {
            builder.ToTable("TestCode_TestQuestion");

            builder.HasKey(t => t.Id);

            builder.HasOne(x => x.TestCodes)
                .WithMany(x => x.TestCode_TestQuestions)
                .HasForeignKey(x => x.TestCodeId)
                .OnDelete(DeleteBehavior.NoAction); ;

            builder.HasOne(x => x.TestQuestion)
                .WithMany(x => x.TestCode_TestQuestions)
                .HasForeignKey(x => x.TestQuestionId)
                .OnDelete(DeleteBehavior.NoAction); ;
        }
    }
}
