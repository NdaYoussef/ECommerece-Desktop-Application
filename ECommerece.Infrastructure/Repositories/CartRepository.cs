using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        ECommerceDbContext _Context;
        public CartRepository(ECommerceDbContext Context)
        {
            _Context = Context;
        }

        public Task Add(Cart entity)
        {
            _Context.Carts.Add(entity);
            return Task.CompletedTask;
        }

        public Task Update(Cart entity)
        {
            _Context.Carts.Update(entity);
            return Task.CompletedTask;
        }


        public Task Delete(Cart entity)
        {
            _Context.Carts.Remove(entity);
            return Task.CompletedTask;
        }


        public void ClearCart(int cartId)
        {
            var items = _Context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToList();
            _Context.CartItems.RemoveRange(items);
        }


        public IQueryable<Cart> GetAll()
        {
            return _Context.Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product);
        }

        public Task<Cart> GetById(int id)
        {
            return _Context.Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void SaveChanges()
        {
            _Context.SaveChanges(); 
        }

        public Task Update(Cart entity)
        {
            throw new NotImplementedException();
        }
    }
}

