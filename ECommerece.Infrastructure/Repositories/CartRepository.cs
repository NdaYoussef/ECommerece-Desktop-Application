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
        //AppDbContext _Context;
        //public CartRepository(AppDbContext Context)
        //{
        //    Context = _Context;
        //}

        public Task Add(CartItem item)
        {
            throw new NotImplementedException();

        }

        public Task Update(CartItem item)
        {
            throw new NotImplementedException();

        }

        public Task Remove(int cartItemId)
        {
            throw new NotImplementedException();

        }

        public Task ClearCart(int cartId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CartItem>> GetAll(int cartId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItem> GetCartItemById(int cartItemId)
        {
            throw new NotImplementedException();
        }

       

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        
    }
}

