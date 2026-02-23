using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.Infrastructure.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        { 
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Property(o=>o.ShippingAddress).HasMaxLength(500);
            builder.Property(o=>o.discountcode).HasMaxLength(4);
            //builder.HasOne(o => o.User)
            //       .WithMany(u => u.Orders)
            //       .HasForeignKey(o => o.UserId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(o => o.OrderItems)
            //       .WithOne(oi => oi.Order)
            //       .HasForeignKey(oi => oi.OrderId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
