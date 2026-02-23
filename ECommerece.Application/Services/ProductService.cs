using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Domain.Entities;
using Mapster;

namespace ECommerece.Application.Services
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        readonly IProductRepository Repository = repository;

        public bool AddProduct(ProductCreateDto productDto)
        {
            Product product = productDto.Adapt<Product>();
            try
            {
                return Repository.GetById(product.Id) != null;
            }
            catch (ArgumentNullException _)
            {
                Repository.Add(product);
                return true;
            }
        }

        public bool DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductDetailsDto>? GetLowStockProducts(int threshold = 5)
        {
            throw new NotImplementedException();
        }

        public ProductDetailsDto? GetProductDetails(string label)
        {
            throw new NotImplementedException();
        }

        public ProductDetailsDto? GetProductDetails(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductListDto>? GetProductsByCategory(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public List<ProductListDto>? GetProductsByCategory(string CategoryName)
        {
            throw new NotImplementedException();
        }

        public List<ProductListDto>? GetProductsList()
        {
            throw new NotImplementedException();
        }

        public List<ProductListDto>? SearchProducts(string keyword)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProduct(int id, ProductCreateDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
