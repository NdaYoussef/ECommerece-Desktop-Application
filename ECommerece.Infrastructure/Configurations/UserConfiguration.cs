using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Configurations
{
    public class UserConfiguration :IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> configuration)
        {
            configuration.Property(n => n.Name).IsRequired().HasMaxLength(100);
            configuration.Property(e => e.Email).IsRequired().HasMaxLength(100);
            configuration.HasIndex(e => e.Email).IsUnique();
            configuration.Property(p => p.Password).IsRequired().HasMaxLength(100);
            configuration.Property(d => d.IsDeleted).HasDefaultValue(false);
            configuration.Property(r => r.Role).IsRequired();

        }
    }
}
