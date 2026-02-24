using ECommerece.Application.DTOs;
using ECommerece.Application.DTOs.UserDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.IUserService
{
    public interface IAccountService
    {
        Task<AuthDto> LoginAsync(LoginDto loginDto);

        Task<AuthDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthDto> LogoutAsync();
    }
}
