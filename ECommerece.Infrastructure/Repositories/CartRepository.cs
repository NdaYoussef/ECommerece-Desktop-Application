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
        public void Add(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void AddCart(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void ClearCart(int cartId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteCart(Cart entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cart> GetAllCart()
        {
            throw new NotImplementedException();
        }

        public Cart GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveCartChanges()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateCart(Cart entity)
        {
            throw new NotImplementedException();
        }
    }
}

