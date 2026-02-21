using ECommerece.Application.DTOs;
using ECommerece.Application.IServices.IUserService;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Services.UserServices
{
    public class UserService : IUserService
    {
        public Task<AuthDto> GetUserData(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
