using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.DTOs.ProductDto;

namespace ECommerece.Application.IServices
{
    public interface IProductService
    {
        public List<ProductListDto>? GetProductsList();
        public Task<ProductDetailsDto?> GetProductDetails(string label);
        public Task<ProductDetailsDto?> GetProductDetails(int id);
        public List<ProductListDto>? GetProductsByCategory(int CategoryId);
        public List<ProductListDto>? GetProductsByCategory(string CategoryName);
        public List<ProductListDto>? SearchProducts(string keyword);
        public Task<bool> AddProduct(ProductCreateDto productDto);
        public Task<bool> UpdateProduct(int id, ProductCreateDto productDto);
        public Task<bool> DeleteProduct(int id);
        public List<ProductDetailsDto>? GetLowStockProducts(int threshold = 5);
    }
}
