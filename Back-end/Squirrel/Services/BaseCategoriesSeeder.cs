using DataAccess.Contexts;
using DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Squirrel.Services
{
    public sealed class BaseCategorySeeder
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<BaseCategorySeeder> _logger;
        private readonly IReadOnlyCollection<Category> _baseCategories = new List<Category>
        {
            new Category {Name = "General", Color = "#000000", IsBaseCategory = true },
            new Category {Name = "General2", Color = "#001220", IsBaseCategory = true },
            new Category {Name = "General3", Color = "#FFFFFF", IsBaseCategory = true },
        };

        public BaseCategorySeeder(ApplicationContext context, ILogger<BaseCategorySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> SeedCategories(string id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Include(u => u.Categories)
                    .FirstOrDefaultAsync();

                user.Categories.AddRange(_baseCategories);

                return await _context.SaveChangesAsync() > 0
                    ? Result.Ok(true)
                    : Result.Ok(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail("Error occured while trying to save the added categories");
            }
        }
    }
}
