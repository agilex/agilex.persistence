namespace agilex.persistence
{
    public interface IRepositoryFactory
    {
        IRepository Instance();
    }
}