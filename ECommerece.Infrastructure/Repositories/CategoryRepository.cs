using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
         ECommerceDbContext _dbcontext;
        public CategoryRepository(ECommerceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IQueryable<Category> GetAll()
        {
            return _dbcontext.Categories;
        }
        public async Task<Category> GetById(int id)
        {
            return await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Category> GetByName(string name)
        {
            return await _dbcontext.Categories.FirstOrDefaultAsync(c => c.CategoryName == name);
        }
        public async Task Add(Category category)
        {
           await _dbcontext.Categories.AddAsync(category);
           await _dbcontext.SaveChangesAsync();
        }
        public async Task Update(Category category)
        {
            _dbcontext.Categories.Update(category);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task Delete(Category category)
        {
            category.IsDeleted = true;
            await _dbcontext.SaveChangesAsync();
        }
    }
}
