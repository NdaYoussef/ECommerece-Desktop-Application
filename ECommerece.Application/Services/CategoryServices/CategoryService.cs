using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.ICategoryService;
using ECommerece.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.CategoryServices
{
    public class CategoryService : ICategoryServices
    {
         ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoryDto>> GetAllCategories()
        {
            var catList = await _categoryRepository.GetAll().ToListAsync();
            var returnList = catList.Adapt<List<GetCategoryDto>>();
            return returnList;
        }
        public async Task<GetCategoryDto> GetCategoryById(int id) { 
            
            var cat = await _categoryRepository.GetById(id);
            if (cat is null) {
                throw new Exception($"Category with id {id} not found");
            }
            return cat.Adapt<GetCategoryDto>();
        }
        public async Task<GetCategoryDto> GetCategoryByName(string name)
        {

            var cat = await _categoryRepository.GetByName(name);
            if (cat is null)
            {
                throw new Exception($"Category with name {name} not found");

            }
            return cat.Adapt<GetCategoryDto>();
        }
        public async Task AddCategory(AddCategoryDto CategoryDto) {

            if(string.IsNullOrWhiteSpace(CategoryDto.Name)){
                throw new Exception($"Category name is required");
            }
            var existCategory = _categoryRepository.GetByName(CategoryDto.Name);
            if (existCategory != null)
            {
                throw new Exception($"Category name already exist");
            }
            Category category = new Category() { Name = CategoryDto.Name, Description = CategoryDto.Description };
            await _categoryRepository.Add(category);
        }
        public async Task UpdateCategory(UpdateCategoryDto CategoryDto)
        {
            var category = await _categoryRepository.GetById(CategoryDto.Id);
            category.Name = CategoryDto.Name;
            category.Description = CategoryDto.Description;
            await _categoryRepository.Update(category);
        }
        public async Task DeleteCategory(int id) { 
          var categoryToDelete = await _categoryRepository.GetById(id);
            if (categoryToDelete is null)
            {
                return;
            }
            await _categoryRepository.Delete(categoryToDelete);
        }
    }
}
