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
        public ICartRepository _cartRepository;
        public IGenericRepository<Product, int> _productRepository;

        public CartService(ICartRepository cartRepository,
                           IGenericRepository<Product, int> productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _cartRepository.Add(cart);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == addToCartDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += addToCartDto.Quantity;
            }
            else
            {
                var product = await _productRepository.GetById(addToCartDto.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                cart.CartItems.Add(new CartItem
                {
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    UnitPrice = product.Price
                });
            }

            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item != null)
                {
                    cart.CartItems.Remove(item);
                    await _cartRepository.SaveChangesAsync();
                }
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                cart.CartItems.Clear();
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            return cart.Adapt<CartDTO>();
        }

        public async Task UpdateCartItemAsync(string userId, UpdateCartItemDto updateDto)
        {
            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(ci => ci.Id == updateDto.CartItemId);
                if (item != null)
                {
                    item.Quantity = updateDto.NewQuantity;
                    await _cartRepository.SaveChangesAsync();
                }
            }
        }

    }
}
