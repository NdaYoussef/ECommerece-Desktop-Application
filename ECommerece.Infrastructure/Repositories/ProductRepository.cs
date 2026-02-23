using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;

namespace ECommerece.Infrastructure.Repositories
{
    public class ProductRepository(ECommerceDbContext Context) : IProductRepository
    {
        readonly ECommerceDbContext context = Context;

        public void Add(Product entity)
        {
            context.Products.Add(entity);
        }

        public void Delete(Product entity)
        {
            context.Products.Remove(entity);
        }

        public IQueryable<Product> GetAll()
        {
            return context.Products;
        }

        public Product GetById(int id)
        {
            return context.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public Product? GetProductByLabel(string label)
        {
            return context.Products.Where(p => p.Label == label).FirstOrDefault();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void SoftDelete(int id)
        {
            var existing = GetById(id);
            existing.IsDeleted = true;
            Update(existing);
            SaveChanges();
        }

        public void Update(Product entity)
        {
            context.Products.Update(entity);
        }
    }
}
