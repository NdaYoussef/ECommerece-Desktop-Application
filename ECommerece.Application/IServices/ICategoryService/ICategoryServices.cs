using ECommerece.Application.DTOs.CategoryDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.ICategoryService
{
    public interface ICategoryServices
    {
        public List<GetCategoryDto> GetAllCategories();
        public GetCategoryDto GetCategoryById(int id);
        public GetCategoryDto GetCategoryByName(string name);
        public void AddCategory(AddCategoryDto CategoryDto);
        public void UpdateCategory(UpdateCategoryDto CategoryDto);
        public void DeleteCategory(int id);
    }
}
