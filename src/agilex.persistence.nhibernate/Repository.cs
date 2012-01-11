using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace agilex.persistence.nhibernate
{
    public class Repository : IRepository
    {
        readonly ISession _session;
        ITransaction _transaction;

        public Repository(ISession session)
        {
            _session = session;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_transaction != null) 
                try {_transaction.Commit();} catch(Exception) {}
            _session.Flush();
            _session.Close();
            _session.Dispose();
        }

        #endregion

        #region IRepository Members

        public T Get<T>(Guid id) where T : class
        {
            return _session.Get<T>(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _session.CreateCriteria<T>().List<T>();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _session.Query<T>();
        }

        public void Save<T>(T entity) where T : class
        {
            _session.Save(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _session.Delete(entity);
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            if (!_transaction.WasCommitted) 
                _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        #endregion
    }
}