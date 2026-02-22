using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Enums
{
    public enum PaymentStatus
    {
        Unknown = 0,
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5

    }
}
