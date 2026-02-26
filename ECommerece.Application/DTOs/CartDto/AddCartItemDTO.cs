using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CartDto
{
    public class AddCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
