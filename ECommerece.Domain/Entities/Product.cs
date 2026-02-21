using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerece.Domain.Entities
{
    public class Product : BaseModel.BaseClass<int>
    {
        public required string Label { get; set; }

        public string? Description { get; set; }

        public required decimal Price { get; set; }

        public required int StockQuantity { get; set; }

        // FK
        public required int CategoryId { get; set; }
        // Navigation
        public Category? Category { get; set; }

        // Navigation
        // public List<OrderItem>? OrderItems { get; set; }

        // public string? ImageUrl { get; set; }
    }
}