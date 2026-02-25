using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Domain.Entities;

namespace ECommerece.Application.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        public Task<Product?> GetProductByLabel(string label);

        // TODO if label isn't unique either return list or delete the function

        public Task<bool> SoftDelete(int id);

        // TODO should it be in IGenericRepository<>
    }
}
