using ECommerece.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerece.Domain.Enums;  
namespace ECommerece.Domain.Entities
{
    public class Order :BaseClass<int>
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; private set; } = 0;

        public OrderStatus Status { get; private set; }= OrderStatus.Pending;
        public PaymentStatus paymentstatus { get; set; } = PaymentStatus.Unknown;
        public decimal ShippingCost { get; set; } = 0;
        public string ShippingAddress { get; set; } = string.Empty;
        public string discountcode { get; set; } = string.Empty;


        public string UserId { get; set; }
        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; }= new List<OrderItem>();


        //public decimal CalculateTotalAmount()
        //{
        //    decimal total = 0;
        //    foreach (var item in OrderItems)
        //    {
        //        total += item.Quantity * item.UnitPrice;

        //    }
        //    total += ShippingCost;
        //    if (!string.IsNullOrEmpty(discountcode))
        //    {
        //        // Apply discount 
        //        total *= 0.9m; // Apply a 10% discount
        //    }
        //    return total;
        //}
         
         public OrderStatus setOrerstaus()
        {
            if (Status == OrderStatus.Pending)
            {
                if (paymentstatus == PaymentStatus.Completed)
                    Status = OrderStatus.Processing;
            }
            else if (Status == OrderStatus.Processing)
            {
              //  Thread.Sleep(2000); // Simulate processing time
                Status = OrderStatus.Shipped;
            }
            else if (Status == OrderStatus.Shipped)
            {
              //  Thread.Sleep(3000); // Simulate delivery time
                Status = OrderStatus.Delivered;
            }
            return Status;
        }
    }
}
