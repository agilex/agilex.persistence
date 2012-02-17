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

        #region IRepository Members

        public void Dispose()
        {
            if (_transaction != null)
                try
                {
                    _transaction.Commit();
                }
                catch (Exception)
                {
                }
            _session.Flush();
            _session.Close();
            _session.Dispose();
        }

        public int Count<T>() where T : class
        {
            return _session.QueryOver<T>().ToRowCountQuery().FutureValue<int>().Value;
        }

        public T Get<T>(Guid id) where T : class
        {
            return _session.Get<T>(id);
        }

        public T Get<T>(int id) where T : class
        {
            return _session.Get<T>(id);
        }

        public T Get<T>(long id) where T : class
        {
            return _session.Get<T>(id);
        }

        public bool Exists<T>(int id) where T : class
        {
            return _session.Get<T>(id) != null;
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
            if (!_transaction.WasCommitted && !_transaction.WasRolledBack)
                _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public T GetOrThrowNotFound<T>(Guid id) where T : class
        {
            return _GetOrThrow<T>(id);
        }

        public T GetOrThrowNotFound<T>(int id) where T : class
        {
            return _GetOrThrow<T>(id);
        }

        public T GetOrThrowNotFound<T>(long id) where T : class
        {
            return _GetOrThrow<T>(id);
        }

        T _GetOrThrow<T>(object id) where T : class
        {
            var e = _session.Get<T>(id);
            if (e == null)
                throw new EntityNotFoundException(string.Format("{0} with Id {1} not found", typeof(T).Name, id));
            return e;
        }

        #endregion
    }
}