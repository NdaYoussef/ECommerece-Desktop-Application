using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Domain.Entities;
using Mapster;

namespace ECommerece.Application.Services.ProductServices
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        readonly IProductRepository Repository = repository;

        public bool AddProduct(ProductCreateDto productDto)
        {
            try
            {
                var product = productDto.Adapt<Product>();
                Repository.Add(product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            var product = Repository.GetById(id);
            if (product.Result != null)
            {
                Repository.SoftDelete(product.Result);
                return true;
            }
            return false;
        }

        public List<ProductDetailsDto>? GetLowStockProducts(int threshold = 5)
        {
            return Repository
                .GetAll()
                .Where(p => p.StockQuantity <= threshold)
                .ProjectToType<ProductDetailsDto>()
                .ToList();
        }

        public ProductDetailsDto? GetProductDetails(string label)
        {
            return Repository.GetProductByLabel(label)?.Adapt<ProductDetailsDto>();
        }

        public ProductDetailsDto? GetProductDetails(int id)
        {
            return Repository.GetById(id)?.Adapt<ProductDetailsDto>();
        }

        public List<ProductListDto>? GetProductsByCategory(int CategoryId)
        {
            return Repository
                .GetAll()
                .Where(p => p.CategoryId == CategoryId)
                .ProjectToType<ProductListDto>()
                .ToList();
        }

        public List<ProductListDto>? GetProductsByCategory(string CategoryName)
        {
            return Repository
                .GetAll()
                .Where(p => (p.Category != null) && p.Category.Name == CategoryName)
                .ProjectToType<ProductListDto>()
                .ToList();
        }

        public List<ProductListDto>? GetProductsList()
        {
            return Repository.GetAll().ProjectToType<ProductListDto>().ToList();
        }

        public List<ProductListDto>? SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetProductsList();

            return Repository
                .GetAll()
                .Where(p => p.Label != null && p.Label.ToLower().Contains(keyword.ToLower()))
                .ProjectToType<ProductListDto>()
                .ToList();
        }

        public bool UpdateProduct(int id, ProductCreateDto productDto)
        {
            try
            {
                var existing = Repository.GetById(id);
                if (existing is null)
                    return false;
                productDto.Adapt(existing);
                Repository.Update(existing.Result);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
