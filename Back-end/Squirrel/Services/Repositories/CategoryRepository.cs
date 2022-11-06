using DataAccess.Contexts;
using DataAccess.Entities;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddRange(IEnumerable<Category> categories)
        {
            _context.Categories.AddRange(categories);
        }
    }
}
