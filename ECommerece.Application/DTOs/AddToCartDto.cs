using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
