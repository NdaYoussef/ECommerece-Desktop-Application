using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category,int>
    {
         Task<Category> GetByName(string name);
    }
}
