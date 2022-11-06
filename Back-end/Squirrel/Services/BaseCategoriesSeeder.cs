using DataAccess.Contexts;
using DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services
{
    public sealed class BaseCategorySeeder
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<BaseCategorySeeder> _logger;
        private readonly IReadOnlyCollection<Category> _baseCategories = new List<Category>
        {
            new Category {Name = "General", Color = "#000000", IsBaseCategory = true },
            new Category {Name = "General2", Color = "#001220", IsBaseCategory = true },
            new Category {Name = "General3", Color = "#FFFFFF", IsBaseCategory = true },
        };

        public BaseCategorySeeder(IUnitOfWork uow, ILogger<BaseCategorySeeder> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<bool>> SeedCategories(string id)
        {
            try
            {
                var userResult = await _uow.UserRepository.GetUserWithCategoriesAsync(id);

                if (!userResult.IsSuccess)
                {
                    return Result.Fail(userResult.Errors);
                }

                _uow.CategoryRepository.AddRange(_baseCategories);

                return Result.Ok(await _uow.Confirm());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail("Error occured while trying to save the added categories");
            }
        }
    }
}
