using System;
using System.Collections.Generic;
using System.Text;
using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.DTOs.UserDto;
using ECommerece.Domain.Entities;
using Mapster;

namespace ECommerece.Application.Mappers
{
    public static class Mapping
    {
        public static void RegisterAllMapping()
        {
            TypeAdapterConfig<User, RegisterDto>.NewConfig();
            TypeAdapterConfig<User, LoginDto>.NewConfig();

            /*
            Start product mapping
            */
            TypeAdapterConfig<Product, ProductListDto>
                .NewConfig()
                .Map(
                    d => d.CategoryName,
                    s => (s.Category != null) ? s.Category.Name : "No Category"
                )
                .Map(d => d.IsInStock, s => s.StockQuantity > 0);

            TypeAdapterConfig<Product, ProductDetailsDto>
                .NewConfig()
                .Map(
                    d => d.CategoryName,
                    s => (s.Category != null) ? s.Category.Name : "No Category"
                );
            /*
            End product mapping
            */
        }
    }
}
