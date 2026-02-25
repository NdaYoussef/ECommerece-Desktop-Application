using ECommerece.Domain.Entities;
using ECommerece.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.OrderDtos
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
       
    }
}

