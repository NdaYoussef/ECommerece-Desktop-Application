using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.ICategoryService;
using ECommerece.Domain.Entities;
using Mapster;
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

        public List<GetCategoryDto> GetAllCategories()
        {
            var catList = _categoryRepository.GetAll().ToList();
            var returnList = catList.Adapt<List<GetCategoryDto>>();
            return returnList;
        }
        public GetCategoryDto GetCategoryById(int id) { 
            
            var cat = _categoryRepository.GetById(id);
            if (cat is null) {
                throw new Exception($"Category with id {id} not found");
            }
            return cat.Adapt<GetCategoryDto>();
        }
        public GetCategoryDto GetCategoryByName(string name)
        {

            var cat = _categoryRepository.GetByName(name);
            if (cat is null)
            {
                throw new Exception($"Category with name {name} not found");

            }
            return cat.Adapt<GetCategoryDto>();
        }
        public void AddCategory(AddCategoryDto CategoryDto) {
            Category category = new Category() { CategoryName = CategoryDto.CategoryName, CategoryDescription = CategoryDto.CategoryDescription };
            _categoryRepository.Add(category);
        }
        public void UpdateCategory(UpdateCategoryDto CategoryDto)
        {
            var category = _categoryRepository.GetById(CategoryDto.Id);
            category.CategoryName = CategoryDto.CategoryName;
            category.CategoryDescription = CategoryDto.CategoryDescription;
            _categoryRepository.Update(category);
        }
        public void DeleteCategory(int id) { 
          var categoryToDelete = _categoryRepository.GetById(id);
            if (categoryToDelete is null)
            {
                return;
            }
            _categoryRepository.Delete(categoryToDelete);
        }
    }
}
