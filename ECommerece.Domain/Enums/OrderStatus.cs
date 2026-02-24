using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public enum OrderStatus
    {
        Pending,     
        Approved,      
        Rejected,      
        Processing,    
        Shipped,
        Delivered,
        Cancelled
    }
}
