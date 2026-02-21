using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity , Tkey> where TEntity : class
    {
        public IQueryable<TEntity> GetAll();
        public TEntity GetById(Tkey id);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

    }
}
