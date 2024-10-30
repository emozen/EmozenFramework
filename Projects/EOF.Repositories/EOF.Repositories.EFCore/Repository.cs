using EOF.Repositories.EFCore.ExceptionHandling;
using EOF.Repositories.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EOF.Repositories.EFCore
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new DatabaseException("dbContext can not be null.");

            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        #region IRepository Members
        public IQueryable<TEntity> QueryAll()
        {
            return _dbSet.AsQueryable();
        }
        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public async Task<TEntity> GetByIdAsync(object key)
        {
            var entity = await _dbSet.FindAsync(key);
            if (entity == null)
            {
                throw new InvalidOperationException("GetById is error !!");
            }
            return entity;
        }
        public async Task AddAsync(TEntity entity)
        {
            //var cloned = _dbContext.Entry(entity).CurrentValues.Clone().ToObject();
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new CrudException("Error in add", ex);
            }

        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw new CrudException("Error in addRange", ex);
            }
        }
        public void UpdateField(TEntity orjinalEntity, TEntity updateEntity)
        {
            try
            {
                if (updateEntity == null)
                    throw new DatabaseException(nameof(updateEntity));

                if (orjinalEntity == null)
                    throw new DatabaseException(nameof(orjinalEntity));

                var entityEntry = _dbContext.Entry(orjinalEntity);

                // Sadece değişiklik olan ve null olmayan değerleri kopyala
                foreach (var property in _dbContext.Entry(updateEntity).Properties)
                {
                    var originalValue = entityEntry.Property(property.Metadata.Name).CurrentValue;
                    var setValue = property.CurrentValue;

                    if (setValue != null && !Equals(originalValue, setValue))
                    {
                        entityEntry.Property(property.Metadata.Name).CurrentValue = setValue;
                        entityEntry.Property(property.Metadata.Name).IsModified = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CrudException("Error in update", ex);
            }
        }
        public void UpdateFull(TEntity entity, string modifiedFieldName = "ModifiedTime")
        {
            if (entity.GetType().GetProperty(modifiedFieldName) != null)
            {
                TEntity _entity = entity;
                _entity.GetType().GetProperty(modifiedFieldName).SetValue(_entity, DateTime.UtcNow);
            }

            _dbContext.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new CrudException(nameof(entity) + " Provided entity is null");
            }

            try
            {
                var deletedTimeProperty = entity.GetType().GetProperty("DeletedTime");

                if (deletedTimeProperty != null && deletedTimeProperty.PropertyType == typeof(DateTime?))
                {
                    // "DeletedTime" property'si var ve DateTime türünde ise güncelleme yap
                    deletedTimeProperty.SetValue(entity, DateTime.UtcNow);
                    _dbContext.Set<TEntity>().Update(entity);
                }
                else
                {
                    // "DeletedTime" property'si yoksa entity'yi doğrudan sil
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
            catch (Exception ex)
            {
                throw new CrudException("Error in delete", ex);
            }
        }
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var deletedTimeProperty = entity.GetType().GetProperty("DeletedTime");

                if (deletedTimeProperty != null && deletedTimeProperty.PropertyType == typeof(DateTime?))
                {
                    // "DeletedTime" property'si var ve DateTime türünde ise güncelleme yap
                    deletedTimeProperty.SetValue(entity, DateTime.UtcNow);
                    _dbContext.Set<TEntity>().Update(entity);
                }
                else
                {
                    // "DeletedTime" property'si yoksa entity'yi doğrudan sil
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
        #endregion
    }
}
