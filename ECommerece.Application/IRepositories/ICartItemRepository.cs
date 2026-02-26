using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartItemRepository : IGenericRepository<CartItem, int>
    {
        Task<CartItem> GetByCartAndProductAsync(int cartId, int productId);
    }
}
