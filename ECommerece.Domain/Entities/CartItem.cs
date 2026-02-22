using ECommerece.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class CartItem : BaseClass<int>
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
