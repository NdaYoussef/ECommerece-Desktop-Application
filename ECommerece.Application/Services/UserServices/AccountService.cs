using ECommerece.Application.DTOs;
using ECommerece.Application.DTOs.UserDto;
using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices.IUserService;
using ECommerece.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.UserServices
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task<AuthDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
                return new AuthDto
                {
                    IsAuthenticated = false,
                    Message = "Invalid email or password"
                };

            var verify = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                loginDto.Password);

            if (verify == PasswordVerificationResult.Failed)
                return new AuthDto
                {
                    IsAuthenticated = false,
                    Message = "Invalid email or password"
                };

            return new AuthDto
            {
                IsAuthenticated = true,
                Message = "Login successful"
            };
        }

        public async Task<AuthDto> LogoutAsync()
        {
            return await Task.FromResult(new AuthDto
            {
                IsAuthenticated = false,
                Message = "Logged out successfully"
            });
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (existingUser != null)
                return new AuthDto
                {
                    IsAuthenticated = false,
                    Message = "User already exists"
                };

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerDto.Email,
                Name = registerDto.Name,
                Role = registerDto.Role,
                
            };

            user.Password = _passwordHasher.HashPassword(user, registerDto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new AuthDto
            {
                IsAuthenticated = true,
                Message = "Register successful"
            };
        }
    }
}
