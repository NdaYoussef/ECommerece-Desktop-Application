using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerece.Infrastructure.Configurations
{
    public class OrderItemconfiguration: IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(oi => !oi.IsDeleted);

            // OrderItem- product
            builder.HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);


            //orderItem - Order  
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    
    }
}
