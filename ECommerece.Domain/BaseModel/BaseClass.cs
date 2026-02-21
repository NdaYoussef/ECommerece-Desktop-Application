using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Domain.BaseModel
{
    public class BaseClass<Tkey>
    {
        public Tkey Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
