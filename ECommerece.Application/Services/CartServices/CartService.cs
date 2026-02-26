using ECommerece.Application.DTOs.CartDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.ICartService;
using ECommerece.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.CartServices
{
    public class CartService : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task AddToCartAsync(string userId, AddCartItemDTO addCartItemDTO)
        {
            // 1. Get existing cart or create a new one
            var cart = await _cartRepository.GetAll()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.Add(cart);
                await _cartRepository.SaveChangesAsync(); // flush to get cart.Id
            }

            // 2. Check if this product is already in the cart
            var existingItem = await _cartItemRepository
                .GetByCartAndProductAsync(cart.Id, addCartItemDTO.ProductId);

            if (existingItem != null)
            {
                // 3a. Already exists — increment quantity only
                existingItem.Quantity += addCartItemDTO.Quantity;
                await _cartItemRepository.Update(existingItem);
            }
            else
            {
                // 3b. New item — map DTO to entity and assign CartId
                var newItem = addCartItemDTO.Adapt<CartItem>();
                newItem.CartId = cart.Id;
                await _cartItemRepository.Add(newItem);
            }

            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetAll()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            var item = await _cartItemRepository
                .GetByCartAndProductAsync(cart.Id, productId);

            if (item != null)
            {
                await _cartItemRepository.Delete(item);
                await _cartItemRepository.SaveChangesAsync();
            }
        }


        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return;

            foreach (var item in cart.CartItems.ToList())
                await _cartItemRepository.Delete(item);

            await _cartItemRepository.SaveChangesAsync();
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return null;

            return cart.Adapt<CartDTO>();
        }

        public async Task UpdateCartItemAsync(string userId, UpdateCartItemDto updateDto)
        {
            var cartItem = await _cartItemRepository.GetById(updateDto.CartItemId);

            if (cartItem == null) return;

            cartItem.Quantity = updateDto.NewQuantity;
            await _cartItemRepository.Update(cartItem);
            await _cartItemRepository.SaveChangesAsync();
        }

    }
}
