using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItem> cartItems { get; set; }
        //public Customer customer { get; set; }

    }
}
