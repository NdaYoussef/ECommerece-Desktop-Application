using System;

namespace ECommerece.Application.DTOs.ProductDto
{
    public class SummaryDto
    {

        public required string Label { get; set; }

        public required decimal Price { get; set; }

        public required int StockQuantity { get; set; }

        // public string? ImageUrl { get; set; }

        public required bool IsDeleted { get; set; }

        public required int CategoryId { get; set; }

    }
}