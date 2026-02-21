using ECommerece.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class Category : BaseClass<int>
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        //public List<Product> products { get; set; }
    }
}
