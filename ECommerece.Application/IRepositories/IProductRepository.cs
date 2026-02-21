using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.Interfaces.Repositories;
using ECommerece.Domain.Entities;

namespace ECommerece.Application.IRepositories
{
    public interface IProductRepository:IGenericRepository<Product,int>
    {
            
            public Product GetProductByLabel(string label);
            public bool SoftDelete(int id);
    }
}