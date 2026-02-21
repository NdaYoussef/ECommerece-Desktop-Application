using System;
using System.ComponentModel.DataAnnotations;
public class Product:ECommerece.Domain.BaseModel.BaseClass
{
    public int Id { get; set; }

    public required string Label { get; set; }

    public string? Description { get; set; }

    public required decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    // FK
    public required int CategoryId { get; set; }
    // Navigation
    public Category? Category { get; set; }

    // Navigation
    public List<OrderItem>? OrderItems { get; set; }

}
