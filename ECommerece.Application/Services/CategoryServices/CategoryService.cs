using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.ICategoryService;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.CategoryServices
{
    public class CategoryService 
    {
        public ICategoryRepository _categoryRepository;
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

    }
}
