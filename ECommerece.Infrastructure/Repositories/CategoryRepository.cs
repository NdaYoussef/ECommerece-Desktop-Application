using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
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
        public Category GetById(int id)
        {
            return _dbcontext.Categories.FirstOrDefault(c => c.Id == id);
        }
        public Category GetByName(string name)
        {
            return _dbcontext.Categories.FirstOrDefault(c => c.CategoryName == name);
        }
        public void Add(Category category)
        {
            _dbcontext.Categories.Add(category);
            _dbcontext.SaveChanges();
        }
        public void Update(Category category)
        {
            _dbcontext.Categories.Update(category);
            _dbcontext.SaveChanges();
        }
        public void Delete(Category category)
        {
            category.IsDeleted = true;
            _dbcontext.SaveChanges();
        }
    }
}
