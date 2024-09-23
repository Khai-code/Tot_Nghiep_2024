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
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.User)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
