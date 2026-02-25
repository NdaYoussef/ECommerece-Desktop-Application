using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);
            
            builder.Property(ci => ci.Quantity)
                .IsRequired();

            builder.Property(ci => ci.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(ci => !ci.IsDeleted);



            //// Many-to-One: CartItem -> Product
            //here error because Product didn't used any property related to cart

            //cartItems - Product
            builder.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //cartItems - Cart
            builder.HasOne(c => c.Cart)
                .WithMany(ci=>ci.CartItems)
                .HasForeignKey(f=>f.CartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
