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
        public ProductDetailsDto? GetProductDetails(string label);
        public ProductDetailsDto? GetProductDetails(int id);
        public List<ProductListDto>? GetProductsByCategory(int CategoryId);
        public List<ProductListDto>? GetProductsByCategory(string CategoryName);
        public List<ProductListDto>? SearchProducts(string keyword);
        public bool AddProduct(ProductCreateDto productDto);
        public bool UpdateProduct(int id, ProductCreateDto productDto);
        public bool DeleteProduct(int id);
        List<ProductDetailsDto>? GetLowStockProducts(int threshold = 5);
    }
}
