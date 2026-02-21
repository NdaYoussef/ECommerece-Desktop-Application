using ECommerece.Application.DTOs;
using ECommerece.Application.DTOs.UserDto;
using ECommerece.Application.IServices.IUserService;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.UserServices
{
    public class AccountService : IAccountService
    {
        public Task<AuthDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthDto> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
