using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.DTOs
{
    public class AuthDto
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public object? Data { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
