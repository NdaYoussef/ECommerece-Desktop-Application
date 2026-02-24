using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.CartDto
{
    public class UpdateCartItemDto
    {
        public int CartItemId { get; set; }
        public int NewQuantity { get; set; }
    }
}
