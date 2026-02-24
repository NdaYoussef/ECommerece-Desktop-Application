using ECommerece.Domain.Entities;
using ECommerece.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.OrderDtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Username { get; set; } 
        public string ShippingAddress { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string? discountcode { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime OrderDate { get; set; }

        //public List<OrderItemDto> Items { get; set; } 
    }
}
