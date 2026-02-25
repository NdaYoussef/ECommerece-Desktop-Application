using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.IOrderService
{
    public interface IOrdercustomerService
    {
         Task<List<OrderDto>> getMyOrdersByStatusAsync(OrderStatus status);

         Task<OrderDto> createOrder(CreateOrderDto order);
         Task<List<OrderDto>> getmyOrdersByDaterangeAsync(DateTime startdate, DateTime EndDate);
         Task<List<OrderDto>> getMyOrdersAsync();
         Task UpdateOrderDetailsAsync(UpdateOrderDto order);
         
        Task CancelOrderAsync(int orderId);
    }
}
