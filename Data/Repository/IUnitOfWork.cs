namespace Dashboard.Data.Repository
{
    public interface IUnitOfWork
    {
       ICategoryRepository Category { get; }
       IProductRepository Product { get; }
       IRoleRepository Role { get; }
       IUserRepository User { get; }
        ICartRepository Cart { get; }
        void Save();
       
    }
}
