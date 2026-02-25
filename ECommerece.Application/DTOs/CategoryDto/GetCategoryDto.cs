using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CategoryDto
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int ProductCount { get; set; }
    }
}
