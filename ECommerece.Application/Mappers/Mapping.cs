using ECommerece.Application.DTOs.CartDto;
﻿using ECommerece.Application.DTOs.CategoryDto;
using ECommerece.Application.DTOs.OrderDtos;
using ECommerece.Application.DTOs.UserDto;
using ECommerece.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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
                .Map(dest => dest.ProductName, src => src.Product.Label)
                .Map(dest => dest.ProductImage, src => src.Product.ImageUrl)
                .Map(dest => dest.TotalPrice, src => src.Quantity * src.UnitPrice);

            TypeAdapterConfig<Category, GetCategoryDto>.NewConfig()
                .Map(dest => dest.ProductCount, src => src.Products != null ? src.Products.Count : 0);
        }

    }
}
