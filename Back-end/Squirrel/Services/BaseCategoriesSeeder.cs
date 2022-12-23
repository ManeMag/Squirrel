using DataAccess.Entities;
using FluentResults;
using Microsoft.Extensions.Localization;
using Squirrel.Extensions;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services
{
    public sealed class BaseCategorySeeder
    {
        private const string MainPresentationColor = "#FF9B3F";
        private readonly IStringLocalizer<SharedResource> _localizer;

        private readonly IUnitOfWork _uow;
        private readonly ILogger<BaseCategorySeeder> _logger;
        private readonly IReadOnlyCollection<Category> _baseCategories = new List<Category>
        {
            new Category { Name = "Other", Color = MainPresentationColor, IsBaseCategory = true },
            new Category { Name = "Test", Color = MainPresentationColor, IsBaseCategory = true },
            new Category { Name = "Test2", Color = MainPresentationColor, IsBaseCategory = true },
            new Category { Name = "Test3", Color ="#666666", IsBaseCategory = true, Type = DataAccess.Entities.Type.Income },
        };

        public BaseCategorySeeder(IUnitOfWork uow, ILogger<BaseCategorySeeder> logger, IStringLocalizer<SharedResource> localizer)
        {
            _uow = uow;
            _logger = logger;
            _localizer = localizer;
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

                userResult.Value.Categories!.AddRange(_baseCategories);

                return Result.Ok(await _uow.Confirm());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail("Error occured while trying to save the added categories".Using(_localizer));
            }
        }
    }
}
