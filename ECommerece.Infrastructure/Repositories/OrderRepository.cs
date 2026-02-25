using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        ECommerceDbContext _dbcontext;
        public OrderRepository(ECommerceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IQueryable<Order> GetAll()
        {
            return _dbcontext.Orders;
        }

        public async Task<Order> GetById(int id)
        {
            return await _dbcontext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
        
        public async Task Add(Order order)
        {
            await _dbcontext.Orders.AddAsync(order);

        }
        public async Task Delete(Order order)
        {
            order.IsDeleted = true;
        }
        public async Task Update(Order order)
        {
            _dbcontext.Orders.Update(order);
        }
        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<List<Order>> GetOdersbyuserAsync(User user)
        {
            return await _dbcontext.Orders.Where(o => o.UserId == user.Id).ToListAsync();
        }
        public async Task<Order> GetOrdersWithItemsAsync(int orderId)
        {
            return await _dbcontext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<List<Order>> GetOrdersbystatusAsync(OrderStatus status)
        {
            return await _dbcontext.Orders.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbcontext.Orders.Where(o => o.OrderDate >= startDate || o.OrderDate <= endDate).ToListAsync();
        }
    }
}
