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
        public Task Add(Cart entity)
        {
            throw new NotImplementedException();
        }

        public Task ClearCart(int cartId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Cart entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Cart> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task Update(Cart entity)
        {
            throw new NotImplementedException();
        }
    }
}

