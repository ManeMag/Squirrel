namespace Squirrel.Services.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }

        IUserRepository UserRepository { get; }

        Task<bool> Confirm();
    }
}
