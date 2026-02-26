using ECommerece.Application.IRepositories;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace ECommerece.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ECommerceDbContext _context;

        public CartItemRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public IQueryable<CartItem> GetAll()
            => _context.CartItems.Include(ci => ci.Product);

        public async Task<CartItem> GetById(int id)
            => await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == id);

        public async Task<CartItem> GetByCartAndProductAsync(int cartId, int productId)
            => await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

        public Task Add(CartItem entity)
        {
            _context.CartItems.Add(entity);
            return Task.CompletedTask;
        }

        public Task Update(CartItem entity)
        {
            _context.CartItems.Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(CartItem entity)
        {
            _context.CartItems.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}