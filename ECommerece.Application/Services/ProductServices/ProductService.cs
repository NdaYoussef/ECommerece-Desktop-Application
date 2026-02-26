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

        public async Task<bool> AddProduct(ProductCreateDto productDto)
        {
            try
            {
                var product = productDto.Adapt<Product>();
                await Repository.Add(product);
                await Repository.SaveChangesAsync(); // ← this is what was missing
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await Repository.GetById(id);
            if (product != null)
            {
                Repository.Delete(product);
                await Repository.SaveChangesAsync();
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

        public async Task<ProductDetailsDto?> GetProductDetails(string label)
        {
            return (await Repository.GetProductByLabel(label))?.Adapt<ProductDetailsDto>();
        }

        public async Task<ProductDetailsDto?> GetProductDetails(int id)
        {
            return (await Repository.GetById(id))?.Adapt<ProductDetailsDto>();
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

        public async Task<bool> UpdateProduct(int id, ProductCreateDto productDto)
        {
            // try
            // {
            var existing = await Repository.GetById(id);
            if (existing is null)
                return false;
            productDto.Adapt(existing);
            await Repository.Update(existing);
            await Repository.SaveChangesAsync();
            return true;
            // }
            // catch
            // {
            //     return false;
            // }
        }
    }
}
