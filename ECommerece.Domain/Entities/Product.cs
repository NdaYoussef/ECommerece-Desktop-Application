using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerece.Domain.Entities
{
    public class Product : BaseModel.BaseClass<int>
    {
        public string? Label { get; set; }

        // TODO is label unique?

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        // TODO image url is a path (but where should the folder be?)

        // FK
        public int CategoryId { get; set; }

        // Navigation
        public Category? Category { get; set; }

        // Navigation
        // public List<OrderItem>? OrderItems { get; set; }
    }
}
