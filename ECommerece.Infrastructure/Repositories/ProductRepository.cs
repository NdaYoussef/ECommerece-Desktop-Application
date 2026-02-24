using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerece.Infrastructure.Repositories
{
    public class ProductRepository(ECommerceDbContext Context) : IProductRepository
    {
        readonly ECommerceDbContext context = Context;

        public async Task Add(Product entity)
        {
            await context.Products.AddAsync(entity);
        }

        public Task Delete(Product entity)
        {
            context.Products.Remove(entity);
            return Task.CompletedTask;
            // await context.SaveChangesAsync();
        }

        public IQueryable<Product> GetAll()
        {
            return context.Products;
        }

        public async Task<Product> GetById(int id)
        {
            return await context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetProductByLabel(string label)
        {
            return await context.Products.Where(p => p.Label == label).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task SoftDelete(Product entity)
        {
            entity.IsDeleted = true;
            await Update(entity);
        }

        public Task Update(Product entity)
        // if passed by reference would it not need to be retrived from the database
        {
            context.Update(entity);
            return Task.CompletedTask;
            // await context.SaveChangesAsync();
        }
    }
}
