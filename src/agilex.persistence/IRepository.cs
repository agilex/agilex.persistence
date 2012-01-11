using System;
using System.Collections.Generic;
using System.Linq;

namespace agilex.persistence
{
    public interface IRepository : IDisposable
    {
        T Get<T>(Guid id) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        IQueryable<T> Query<T>() where T : class;
        void Save<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}