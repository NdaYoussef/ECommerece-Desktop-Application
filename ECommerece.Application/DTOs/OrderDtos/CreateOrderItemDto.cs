using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.OrderDtos
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
