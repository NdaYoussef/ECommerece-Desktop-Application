using ECommerece.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs.UserDto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }

    }
}
