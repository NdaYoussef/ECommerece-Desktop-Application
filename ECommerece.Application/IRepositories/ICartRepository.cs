using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartRepository : IGenericRepository<Cart, int>
    {
        public IQueryable<Cart> GetAllCart();
        public Cart GetById(int id);
        public void AddCart(Cart entity);
        public void UpdateCart(Cart entity);
        public void DeleteCart(Cart entity);
        public void SaveCartChanges();

        public void ClearCart(int cartId);
    }
}
