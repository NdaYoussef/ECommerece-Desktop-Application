using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.OrderServices
{
    public class OrderAdminService: IOrderAdminService
    {
        IOrderRepository _orderRepository;
        public OrderAdminService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var ordList = await _orderRepository.GetAll().ToListAsync();
            var returnList = ordList.Adapt<List<OrderDto>>();
            return returnList;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id) 
        {
            var ord = await _orderRepository.GetById(id);
            if (ord is null)
            {
                throw new Exception($"order with id {id} not found");
            }
            return ord.Adapt<OrderDto>();

        }
        public async Task ApproveOrder(OrderDto order) 
        { 
            var ord = await _orderRepository.GetById(order.Id);

            if (ord == null)
                throw new Exception("Order not found");

            if (ord.Status != OrderStatus.Pending)
                throw new Exception("Only pending orders can be approved");

            order.Status = OrderStatus.Approved;

            await _orderRepository.Update(ord);
            
            await _orderRepository.SaveChangesAsync();
        }
        public async Task RejectOrder(OrderDto order) 
        {
            var ord = await _orderRepository.GetById(order.Id);

            if (ord == null)
                throw new Exception("Order not found");

            if (ord.Status != OrderStatus.Pending)
                throw new Exception("Only pending orders can be rejected");
            foreach (var item in order.Items)
            {
                item.Product.Stock += item.Quantity;
            }

            order.Status = OrderStatus.Rejected;

            await _orderRepository.Update(ord);

            await _orderRepository.SaveChangesAsync();
        }

        public async Task<List<OrderDto>> getUserOrdersAsync(int id)
        {
            var orders = await _orderRepository
            .GetAll()
            .Where(o => o.UserId == id.ToString())
            .ToListAsync();

            return orders.Adapt<List<OrderDto>>();
        }

        public async Task<List<OrderDto>> getcustomerOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository
            .GetAll()
            .Where(o => o.Status == status)
            .ToListAsync();

            return orders.Adapt<List<OrderDto>>();
        }

        public async Task<List<OrderDto>> getcustomerOrdersByDaterangeAsync(DateTime start, DateTime End)
        {
            var orders = await _orderRepository
            .GetAll()
            .Where(o => o.OrderDate >= start && o.OrderDate <= End)
            .ToListAsync();

            return orders.Adapt<List<OrderDto>>();
        }
    }
}
