 using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ECommerece.Application.Services.OrderServices
{
    public class OrdercustomerService:IOrdercustomerService 
    {
        IOrderRepository _orderRepository;
        public OrdercustomerService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderDto>> GetAllOrders()
        {   
            var ordList = await _orderRepository.GetAll().ToListAsync();
            var returnList = ordList.Adapt<List<OrderDto>>();
            return returnList;
        }
        public async Task<OrderDto> GetOrderById(int id)
        {

            var ord = await _orderRepository.GetById(id);
            if (ord is null)
            {
                throw new Exception($"order with id {id} not found");
            }
            return ord.Adapt<OrderDto>();
        }
        public Task<List<OrderDto>> getMyOrdersByStatusAsync(OrderStatus status)
        { 
            return _orderRepository.GetAll().Where(o=>o.Status==status).ProjectToType<OrderDto>().ToListAsync();
        }

        public Task<OrderDto> createOrder(CreateOrderDto order)
        {
                var newOrder = order.Adapt<Order>();
                _orderRepository.Add(newOrder);
                _orderRepository.SaveChangesAsync();
                return Task.FromResult(newOrder.Adapt<OrderDto>());
        }
        public Task<List<OrderDto>> getmyOrdersByDaterangeAsync(DateTime startdate, DateTime EndDate)
        {
            var orders= _orderRepository.GetOrdersByDateRangeAsync(startdate, EndDate);
            return Task.FromResult(orders.Adapt<List<OrderDto>>());
        }
        public Task<List<OrderDto>> getMyOrdersAsync()
        { 
            var orders = _orderRepository.GetAll().ToListAsync();
            return Task.FromResult(orders.Adapt<List<OrderDto>>());
        }
        public async Task UpdateOrderDetailsAsync(UpdateOrderDto dto)
        {
            var ord = await _orderRepository.GetById(dto.Id);

            if (ord == null)
                throw new Exception($"order with id {dto.Id} not found");

            if (ord.Status != OrderStatus.Pending)
                throw new Exception("order cannot be updated");

            dto.Adapt(ord);   // DTO -> entity update

            await _orderRepository.Update(ord);
            await  _orderRepository.SaveChangesAsync();
        }
        
        

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetById(orderId);
            if(order==null)
            {
                throw new Exception($"order with id {orderId} not found");
            }
            if (order.Status != OrderStatus.Pending)
            {
                throw new Exception("order cannot be cancelled");
            }
             
                await _orderRepository.Delete(order);

                await _orderRepository.SaveChangesAsync();
      
        }
    

    }
}
