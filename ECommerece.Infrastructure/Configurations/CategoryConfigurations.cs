using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryName).IsRequired().HasMaxLength(50);
            builder.HasIndex(c => c.CategoryName).IsUnique();

            builder.Property(c => c.CategoryDescription).IsRequired(false).HasMaxLength(200);
           
        }
    }
}
