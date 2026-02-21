using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class Cart
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
