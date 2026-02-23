using ECommerece.Domain.BaseModel;
using ECommerece.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.Entities
{
    public class User : BaseClass<string>
    { 
        //public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRoles Role {  get; set; }

        public List<Order> Orders   { get; set; }
       
    }
}
