using System;
using System.Collections.Generic;
using System.Text;
using ECommerece.Domain.BaseModel;
namespace ECommerece.Domain.Entities
{
    public class OrderItem: BaseClass<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
