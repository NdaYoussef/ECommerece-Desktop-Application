using ECommerece.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.CreatedAt).IsRequired();
            
            builder.Property(c => c.UpdatedAt).IsRequired();

            builder.HasMany(c => c.CartItems)
                  .WithOne(ci => ci.Cart)
                  .HasForeignKey(ci => ci.CartId);


        }
    }
}
