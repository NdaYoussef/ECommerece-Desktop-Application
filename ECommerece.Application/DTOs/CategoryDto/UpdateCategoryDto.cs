using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CategoryDto
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
