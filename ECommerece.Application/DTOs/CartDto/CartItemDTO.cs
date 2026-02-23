using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CartDto
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
