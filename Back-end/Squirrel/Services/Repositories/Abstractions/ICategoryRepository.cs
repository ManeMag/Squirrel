using DataAccess.Entities;

namespace Squirrel.Services.Repositories.Abstractions
{
    public interface ICategoryRepository
    {
        public void AddRange(IEnumerable<Category> categories);
    }
}
