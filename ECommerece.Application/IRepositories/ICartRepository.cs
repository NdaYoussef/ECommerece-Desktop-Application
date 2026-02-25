using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartRepository : IGenericRepository<Cart, int>
    {
      
       Task ClearCart(int cartId);

    }
}
