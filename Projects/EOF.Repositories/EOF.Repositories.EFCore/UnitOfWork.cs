using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOF.Repositories.EFCore.Interfaces;
using EOF.Repositories.EFCore.ExceptionHandling;

namespace EOF.Repositories.EFCore
{
    public class UnitOfWork(DbContext dbContext) : IUnitOfWork
    {
        private DbContext _dbContext = dbContext ?? throw new ArgumentNullException("dbContext can not be null.");
        private IDbContextTransaction? _transation;

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(_dbContext);
        }
        public object GetDbContext()
        {
            return _dbContext;
        }
        public async Task BeginNewTransaction()
        {
            if (_dbContext.Database.CurrentTransaction == null)
            {
                try
                {
                    _transation = await _dbContext.Database.BeginTransactionAsync();
                }
                catch
                {
                    throw new DatabaseException("BeginTransaction is not start in EFUnitofWork");
                }
            }
        }

        public async Task RollBackTransaction()
        {
            if (_transation is not null)
            {
                await _transation.RollbackAsync();
            }
            throw new DatabaseException("Transaction is null in RollBackTransaction");
        }
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                //Context boş ise hata fırlatıyoruz
                if (_dbContext == null)
                {
                    throw new DatabaseException("Context is null");
                }

                //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                foreach (var entity in _dbContext.ChangeTracker.Entries())
                {
                    entity.State = EntityState.Detached;
                }

                throw new DatabaseException("Error in SaveChanges", ex);
            }
        }

        public async Task Commit()
        {
            try
            {
                if (_transation == null)
                {
                    throw new DatabaseException("Transation is null in Commit");
                }

                await _transation.CommitAsync();

                foreach (var entity in _dbContext.ChangeTracker.Entries())
                {
                    entity.State = EntityState.Detached;
                }
            }
            catch (Exception ex)
            {
                await RollBackTransaction();
                throw new DatabaseException("Commit is error", ex);
            }
        }

        #region IDisposable Members
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        public IEntity CloneObject(IEntity entity)
        {
            return (IEntity)_dbContext.Entry(entity).CurrentValues.Clone().ToObject();
        }
    }
}
