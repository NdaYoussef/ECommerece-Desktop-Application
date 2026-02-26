using ECommerece.Application.DTOs.ProductDto;
using ECommerece.Application.DTOs;
using ECommerece.Application.DTOs.UserDto;
using ECommerece.Domain.Entities;
using Mapster;
using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.DTOs.CartDto;

namespace ECommerece.Application.Mappers
{
    public static class Mapping
    {
        public static void RegisterAllMapping()
        {
            TypeAdapterConfig<User, RegisterDto>.NewConfig();
            TypeAdapterConfig<User, LoginDto>.NewConfig();

            TypeAdapterConfig<Order, OrderDto>.NewConfig().TwoWays()
                .Map(dest => dest.Items, src => src.OrderItems) ;
            TypeAdapterConfig<CreateOrderDto,Order>.NewConfig();
            TypeAdapterConfig<UpdateOrderDto, Order>.NewConfig();
            TypeAdapterConfig< OrderItem, OrderItemDto>.NewConfig().TwoWays();
            TypeAdapterConfig<CreateOrderItemDto, OrderItem>.NewConfig();

            TypeAdapterConfig<Cart, CartDTO>.NewConfig()
                .Map(dest => dest.TotalAmount, src => src.CartItems.Sum(ci => ci.Quantity * ci.UnitPrice))
                .Map(dest => dest.TotalItems, src => src.CartItems.Sum(ci => ci.Quantity))
                .Map(dest => dest.Items, src => src.CartItems);

            TypeAdapterConfig<CartItem, CartItemDTO>.NewConfig()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.ProductName, src => src.Product.Label)
                .Map(dest => dest.ProductImage, src => src.Product.ImageUrl)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice);

            TypeAdapterConfig<Category, GetCategoryDto>.NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products != null ? src.Products.Count : 0);


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

            TypeAdapterConfig<OrderItemDto, OrderItem>.NewConfig();
        }

    }
}
