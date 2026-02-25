using System;
using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Domain.Entities;
using System.Collections.Generic;
using System.Text;
namespace ECommerece.Application.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order,int>
    {
         Task<List<Order>>GetOdersbyuserAsync(User user);
         Task<Order> GetOrdersWithItemsAsync(int orderId);
         Task<List<Order>> GetOrdersbystatusAsync(OrderStatus status);
         
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
