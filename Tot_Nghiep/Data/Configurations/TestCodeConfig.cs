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
    internal class TestCodeConfig : IEntityTypeConfiguration<TestCode>
    {
        public void Configure(EntityTypeBuilder<TestCode> builder)
        {
            builder.ToTable("TestCode");
            
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Test)
                .WithMany(x => x.TestCodes)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
