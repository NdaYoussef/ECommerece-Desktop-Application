using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CartDto
{
    public  class CartDTO
    {
        public int CartId { get; set; }
        public List<CartItemDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
