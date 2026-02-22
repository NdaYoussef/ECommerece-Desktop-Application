using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartRepository : IGenericRepository<CartItem, int>
    {       
        public void ClearCart(int cartId);
    }
}
