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
    partial class PointTypeConfig : IEntityTypeConfiguration<PointType>
    {
        public void Configure(EntityTypeBuilder<PointType> builder)
        {
            builder.ToTable("PointType");

            builder.HasKey(x => x.Id);
        }
    }
}
