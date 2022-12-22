using DataAccess.Entities;
using FluentAssertions;
using FluentResults;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Squirrel.Services;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.UnitTests.ServiceTests
{
    public sealed class BaseCategorySeederTests
    {
        private readonly BaseCategorySeeder _baseCategorySeeder;
        private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

        public BaseCategorySeederTests()
        {
            var logger = Substitute.For<ILogger<BaseCategorySeeder>>();
            _uow.UserRepository.Returns(_userRepository);
            _uow.CategoryRepository.Returns(_categoryRepository);

            _baseCategorySeeder = new BaseCategorySeeder(_uow, logger);
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnFail_WhenUserNotFound()
        {
            _userRepository.GetUserWithCategoriesAsync(Arg.Any<string>()).Returns(Result.Fail("Error"));

            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnOkAndTrue_WhenChangesAdded()
        {
            _userRepository.GetUserWithCategoriesAsync(Arg.Any<string>())
                .Returns(Result.Ok(new User { Categories = new() }));
            _uow.Confirm().Returns(true);

            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeTrue();
            seedingResult.Value.Should().BeTrue();
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnOkAndFalse_WhenChangesWereNotAdded()
        {
            _userRepository.GetUserWithCategoriesAsync(Arg.Any<string>())
                .Returns(Result.Ok(new User { Categories = new() }));
            _uow.Confirm().Returns(false);

            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeTrue();
            seedingResult.Value.Should().BeFalse();
        }

        [Fact]
        public async Task SeedCategories_ShouldReturnFail_WhenExceptioned()
        {
            _userRepository.GetUserWithCategoriesAsync(Arg.Any<string>())
                .Throws(new Exception());

            var seedingResult = await _baseCategorySeeder.SeedCategories(Guid.NewGuid().ToString());

            seedingResult.IsSuccess.Should().BeFalse();
        }
    }
}
