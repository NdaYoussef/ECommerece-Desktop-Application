using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerece.Application.IRepositories
{
    public interface IProductRepository
    {
        	public IQueryable<Product> GetAllProducts();
            public Product? GetProductById();
            public bool AddProduct(Product product);
            public bool DeleteProduct(int id);
            public bool UpdateProduct(Product product);
            public Product GetProductByLabel(string label);
            public bool SDelete(int id);
    }
}