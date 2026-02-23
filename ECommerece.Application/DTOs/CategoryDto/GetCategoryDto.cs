using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CategoryDto
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
