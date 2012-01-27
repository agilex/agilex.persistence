namespace agilex.persistence
{
    public abstract class CustomRepository
    {
        protected readonly IRepository _repository;

        protected CustomRepository(IRepositoryLocator repositoryLocatorLocator)
        {
            _repository = repositoryLocatorLocator.RepositoryInstance;
        }
    }
}