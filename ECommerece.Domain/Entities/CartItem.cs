using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        //public Product product { get; set; }
        public int CartId { get; set; }
        public Cart cart { get; set; }
    }
}
