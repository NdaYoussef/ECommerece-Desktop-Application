using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Label).HasMaxLength(150).IsRequired();
            builder.HasIndex(p => p.Label).IsUnique();
            builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();
            builder.Property(p => p.StockQuantity).IsRequired();
            builder.ToTable(t => t.HasCheckConstraint("CK_Product_Price_Positive", "[Price] > 0"));
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Product_StockQuantity_Positive", "[StockQuantity] >= 0")
            );
            builder
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(p => p.CartItems)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
