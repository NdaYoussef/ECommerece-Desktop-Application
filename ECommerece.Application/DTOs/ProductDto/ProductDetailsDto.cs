using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerece.Application.DTOs.ProductDto
{
    public class ProductDetailsDto
    {
        public string? Label { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public required int StockQuantity { get; set; }
    }
}
