using ECommerece.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IServices.IUserService
{
    public interface IUserService
    {
        Task<AuthDto> GetUserData(string userId);
    }
}
