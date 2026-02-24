using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICartRepository : IGenericRepository<Cart, int>
    {
      //  public IQueryable<Cart> GetAllCart();
     //   public Cart GetById(int id);

    

       Task ClearCart(int cartId);
    }
}
