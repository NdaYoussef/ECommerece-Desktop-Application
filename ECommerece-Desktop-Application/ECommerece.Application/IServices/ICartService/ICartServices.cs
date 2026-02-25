using ECommerece.Application.DTOs.CartDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.ICartService
{
    public interface ICartServices
    {
        Task<CartDTO> GetCartByUserIdAsync(string userId);
        Task AddToCartAsync(string userId, AddToCartDto addToCartDto);
        Task RemoveFromCartAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
        Task UpdateCartItemAsync(string userId, UpdateCartItemDto updateDto);

    }
}
