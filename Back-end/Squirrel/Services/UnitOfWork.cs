using DataAccess.Contexts;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(
            ApplicationContext context, 
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            UserRepository = userRepository;
        }
        public ICategoryRepository CategoryRepository { get; }

        public IUserRepository UserRepository { get; }

        public async Task<bool> Confirm()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
