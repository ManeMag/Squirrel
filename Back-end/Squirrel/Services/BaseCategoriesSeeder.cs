using DataAccess.Entities;
using FluentResults;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services
{
    public sealed class BaseCategorySeeder
    {
        private const string MainPresentationColor = "#FF9B3F";

        private readonly IUnitOfWork _uow;
        private readonly ILogger<BaseCategorySeeder> _logger;
        private readonly IReadOnlyCollection<Category> _baseCategories = new List<Category>
        {
            new Category { Name = "Other", Color = MainPresentationColor, IsBaseCategory = true },
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

                userResult.Value.Categories.AddRange(_baseCategories);

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
