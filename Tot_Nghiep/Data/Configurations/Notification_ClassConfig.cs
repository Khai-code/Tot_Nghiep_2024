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
    internal class Notification_ClassConfig : IEntityTypeConfiguration<Notification_Class>
    {
        public void Configure(EntityTypeBuilder<Notification_Class> builder)
        {
            builder.ToTable("Notification_Class");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Notification_Classe)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Class)
                .WithMany(x => x.Notification_Classe)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
