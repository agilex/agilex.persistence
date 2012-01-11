using NHibernate;

namespace agilex.persistence.nhibernate
{
    public class RepositoryFactory : IRepositoryFactory
    {
        readonly ISessionFactory _sessionFactory;

        public RepositoryFactory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IRepository Instance()
        {
            return new Repository(_sessionFactory.OpenSession());
        }
    }
}