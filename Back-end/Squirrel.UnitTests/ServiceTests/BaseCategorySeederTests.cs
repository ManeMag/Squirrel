using DataAccess.Contexts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Squirrel.Services;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.UnitTests.ServiceTests
{
    public sealed class BaseCategorySeederTests
    {
        private readonly BaseCategorySeeder _baseCategorySeeder;
        private readonly IUnitOfWork _uow;

        public BaseCategorySeederTests()
        {
            var logger = Substitute.For<ILogger<BaseCategorySeeder>>();
            _uow = Substitute.For<IUnitOfWork>();

            _baseCategorySeeder = new BaseCategorySeeder(_uow, logger);
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnOkWithTrue_WhenCategoriesSeededSuccessfully()
        {
            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeTrue();
            seedingResult.Value.Should().BeTrue();
        }
    }
}
