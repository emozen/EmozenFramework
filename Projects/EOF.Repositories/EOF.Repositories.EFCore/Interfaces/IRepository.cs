using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EOF.Repositories.EFCore.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> QueryAll();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(object key);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void UpdateField(TEntity orjinalEntity, TEntity updateEntity);
        void UpdateFull(TEntity entity, string modifiedFieldName = "ModifiedTime");
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
