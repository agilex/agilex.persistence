namespace agilex.persistence
{
    public interface IRepositoryLocator
    {
        IRepository RepositoryInstance { get; }
    }
}