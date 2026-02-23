using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Configurations
{
    public class UserConfigurations :IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(n => n.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(p => p.Password).IsRequired().HasMaxLength(100);
            builder.Property(d => d.IsDeleted).HasDefaultValue(false);
            builder.Property(r => r.Role).IsRequired();


            //user - order
            builder.HasMany(o => o.Orders)
                    .WithOne(u => u.User)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);




        }
    }
}
