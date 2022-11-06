using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Squirrel.Data.Contexts;
using Squirrel.Services;

namespace Squirrel.UnitTests.ServiceTests
{
    public sealed class BaseCategorySeederTests
    {
        private readonly BaseCategorySeeder _baseCategorySeeder;
        private readonly ApplicationContext _context;

        public BaseCategorySeederTests()
        {
            var logger = Substitute.For<ILogger<BaseCategorySeeder>>();
            _context = Substitute.For<ApplicationContext>();

            _baseCategorySeeder = new BaseCategorySeeder(_context, logger);
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnOkWithTrue_WhenCategoriesSeededSuccessfully()
        {
            _context.SaveChangesAsync().Returns(1);

            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeTrue();
            seedingResult.Value.Should().BeTrue();
        }
    }
}
