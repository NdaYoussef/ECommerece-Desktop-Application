using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.IOrderService
{
    public interface IOrderAdminService
    {
         Task<List<OrderDto>> GetAllOrdersAsync();

         Task<OrderDto> GetOrderByIdAsync(int id);
         Task ApproveOrder(OrderDto order);
        Task RejectOrder(OrderDto order);

        Task<List<OrderDto>> getUserOrdersAsync(int id);

         Task<List<OrderDto>> getcustomerOrdersByStatusAsync(OrderStatus status);

        Task<List<OrderDto>> getcustomerOrdersByDaterangeAsync(DateTime startdate,DateTime EndDate);
    }
}
