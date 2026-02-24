using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerece.Application.IRepositories
{
    public interface IGenericRepository<TEntity , Tkey> where TEntity : class
    {
         IQueryable<TEntity> GetAll();
         Task<TEntity> GetById(Tkey id);
         Task Add(TEntity entity);
         Task Update(TEntity entity);
         Task Delete(TEntity entity);

        Task SaveChangesAsync();
    }
}
