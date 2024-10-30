using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOF.Repositories.EFCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity;
        public object GetDbContext();
        /// <summary>
        /// Transaction açar
        /// </summary>
        Task BeginNewTransaction();
        /// <summary>
        /// Transaction açıldı ise hata olması durumunda değişiklikleri geri alır
        /// </summary>
        /// <returns></returns>
        Task RollBackTransaction();
        /// <summary>
        /// Yapılan işlemleri db ye gönderir.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Transaction işlemi başlatıldı ise commit ile transactionu kapatır.
        /// </summary>
        Task Commit();

        /// <summary>
        /// Obje kopyalamak
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IEntity CloneObject(IEntity entity);
    }
}
