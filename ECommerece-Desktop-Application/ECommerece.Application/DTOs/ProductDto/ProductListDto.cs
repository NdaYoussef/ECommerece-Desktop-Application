using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerece.Application.DTOs.ProductDto
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string? Label { get; set; }

        public decimal Price { get; set; }

        public bool IsInStock { get; set; }

        public string? ImageUrl { get; set; }

        public string? CategoryName { get; set; }
    }
}
