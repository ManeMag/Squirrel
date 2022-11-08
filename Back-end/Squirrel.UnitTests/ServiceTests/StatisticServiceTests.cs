using AutoMapper;
using DataAccess.Entities;
using FluentAssertions;
using FluentResults;
using NSubstitute;
using Squirrel.Mapping;
using Squirrel.Services;
using Squirrel.Services.Abstractions;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.UnitTests.ServiceTests
{
    public class StatisticServiceTests
    {
        private readonly IStatisticService _statisticService;
        private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

        public StatisticServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();

            _uow.UserRepository.Returns(_userRepository);
            _statisticService = new StatisticService(_uow, mapper);
        }

        [Fact]
        public async Task GetStatisticsForPeriod_ShouldReturnFail_WhenStartDateGreaterThanEndDate()
        {
            var statisticsResult = await _statisticService
                .GetStatisticsForPeriod(Guid.NewGuid().ToString(),
                DateTime.UtcNow,
                DateTime.UtcNow - TimeSpan.FromDays(30));

            statisticsResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task GetStatisticsForPeriod_ShouldReturnFail_WhenEndDateInFuture()
        {
            var statisticsResult = await _statisticService
                .GetStatisticsForPeriod(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow,
                DateTime.UtcNow + TimeSpan.FromDays(30));

            statisticsResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task GetStatisticsForPeriod_ShouldReturnFail_WhenGettingTransactionsFailed()
        {
            _userRepository
                .GetUserWithTransactionsAsync(Arg.Any<string>())
                .ReturnsForAnyArgs(Result.Fail(string.Empty));

            var statisticsResult = await _statisticService
                .GetStatisticsForPeriod(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow - TimeSpan.FromDays(30),
                DateTime.UtcNow);

            statisticsResult.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task GetStatisticsForPeriod_ShouldReturnOkWithTransactionResult_ForValidData()
        {
            _userRepository
                .GetUserWithTransactionsAsync(Arg.Any<string>())
                .ReturnsForAnyArgs(Result.Ok(GetValidUser()));

            var statisticsResult = await _statisticService
                .GetStatisticsForPeriod(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow - TimeSpan.FromDays(30),
                DateTime.UtcNow);

            statisticsResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetStatisticsForPeriod_ShouldCalculateCorrect_ForValidData()
        {
            _userRepository
                .GetUserWithTransactionsAsync(Arg.Any<string>())
                .ReturnsForAnyArgs(Result.Ok(GetValidUser()));

            var statisticsResult = await _statisticService
                .GetStatisticsForPeriod(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow - TimeSpan.FromDays(365),
                DateTime.UtcNow);

            var impact = statisticsResult.Value.Impact;

            statisticsResult.IsSuccess.Should().BeTrue();

            impact.ToList()[0].TotalPercentage.Should().Be(30);
            impact.ToList()[1].TotalPercentage.Should().Be(50);
            impact.ToList()[2].TotalPercentage.Should().Be(20);

            impact.Select(t => t.TransactionsCount).Sum().Should().Be(6);

            impact.Select(t => t.TotalMoney).Sum().Should().Be(1000);
        }

        /// <summary>
        /// 6 total transactions
        /// 1000 total amount for all transactions
        /// 3 categories
        /// 2 transactions per category
        /// percentage: 30, 50, 20
        /// </summary>
        /// <returns></returns>
        private User GetValidUser() =>
            new User
            {
                Categories = new List<Category>
                {
                    new Category
                    {
                        Id = 1,
                        Transactions = new List<Transaction>
                        {
                            new Transaction
                            {
                                Amount = 100,
                                CategoryId = 1,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(30),
                            },
                            new Transaction
                            {
                                Amount = 200,
                                CategoryId = 1,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(60),
                            }
                        }
                    },
                    new Category
                    {
                        Id = 2,
                        Transactions = new List<Transaction>
                        {
                            new Transaction
                            {
                                Amount = 300,
                                CategoryId = 2,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(90),
                            },
                            new Transaction
                            {
                                Amount = 200,
                                CategoryId = 2,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(120),
                            }
                        }
                    },
                    new Category
                    {
                        Id = 3,
                        Transactions = new List<Transaction>
                        {
                            new Transaction
                            {
                                Amount = 100,
                                CategoryId = 3,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(200),
                            },
                            new Transaction
                            {
                                Amount = 100,
                                CategoryId = 3,
                                Time = DateTime.UtcNow - TimeSpan.FromDays(200),
                            }
                        }
                    }
                }
            };
    }
}
