using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CartDto
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
