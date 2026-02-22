using ECommerece.Application.DTOs.CategoryDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.ICategoryService
{
    public interface ICategoryServices
    {
         Task<List<GetCategoryDto>> GetAllCategories();
         Task<GetCategoryDto> GetCategoryById(int id);
         Task<GetCategoryDto> GetCategoryByName(string name);
         Task AddCategory(AddCategoryDto CategoryDto);
         Task UpdateCategory(UpdateCategoryDto CategoryDto);
         Task DeleteCategory(int id);
    }
}
