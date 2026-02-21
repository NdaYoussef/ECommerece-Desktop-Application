using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartRepository
    {
        public Task<List<CartItem>> GetAll(int cartId);
        public Task<CartItem> GetCartItemById(int cartItemId );
        public Task Add(CartItem item);
        public Task Update(CartItem item);
        public Task Remove(int cartItemId);
        public Task ClearCart(int cartId);
        public Task SaveChanges();
    }
}
