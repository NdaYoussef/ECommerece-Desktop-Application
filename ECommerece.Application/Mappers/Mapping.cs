using ECommerece.Application.DTOs.UserDto;
using ECommerece.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Mappers
{
    public static class Mapping
    {
        public static void RegisterAllMapping()
        {
            TypeAdapterConfig<User, RegisterDto>.NewConfig();
            TypeAdapterConfig<User, LoginDto>.NewConfig();
        }

    }
}
