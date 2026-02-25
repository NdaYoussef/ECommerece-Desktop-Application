using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
       
        public string ShippingAddress { get; set; } = default!;
        public decimal ShippingCost { get; set; } = 0;
        public string? discountcode { get; set; }
       public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
