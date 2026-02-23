using ECommerece.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class Category : BaseClass<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<Product> Products { get; set; }
    }
}
