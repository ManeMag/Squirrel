using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Localization;
using Squirrel.Extensions;
using Squirrel.Responses.Statistics;
using Squirrel.Responses.Transaction;
using Squirrel.Services.Abstractions;
using Squirrel.Services.Repositories.Abstractions;
using static Squirrel.Responses.Statistics.StatisticsViewModel;

namespace Squirrel.Services
{
    public sealed class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public StatisticService(
            IUnitOfWork uow,
            IMapper mapper,
            IStringLocalizer<SharedResource> localizer)
        {
            _uow = uow;
            _mapper = mapper;
            _localizer = localizer;
        }

        private DateTime UtcNow => DateTime.UtcNow;

        private DateTime MonthAgo => DateTime.UtcNow - TimeSpan.FromDays(30);

        private DateTime HalfYearAgo => DateTime.UtcNow - TimeSpan.FromDays(180);

        private Result DatesAreValid(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) return Result.Fail("Start date cannot be greater than end date".Using(_localizer));
            if (endDate > UtcNow) return Result.Fail("End date cannot be in future".Using(_localizer));

            return Result.Ok();
        }

        public async Task<Result<StatisticsViewModel>> GetStatisticsForPeriod(
            string userId, DateTime startDate, DateTime endDate)
        {
            var validationResult = DatesAreValid(startDate, endDate);

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            var transactionsResult = await GetTransactionsForPeriod(userId, startDate, endDate);

            if (transactionsResult.IsFailed)
            {
                return Result.Fail(transactionsResult.Errors);
            }

            return Result.Ok(new StatisticsViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Impact = CalculateImpact(transactionsResult.Value)
            });
        }

        public async Task<Result<StatisticsViewModel>> GetStatisticsForMonth(string userId) =>
            await GetStatisticsForPeriod(userId, MonthAgo, UtcNow);

        public async Task<Result<StatisticsViewModel>> GetStatisticsForHalfYear(string userId) =>
            await GetStatisticsForPeriod(userId, HalfYearAgo, UtcNow);

        private IEnumerable<TransactionImpact> CalculateImpact(IEnumerable<TransactionViewModel> transactions)
        {
            var total = transactions.Sum(t => t.Amount);

            var transactionGroupedByCategoryId = transactions
                .GroupBy(t => t.CategoryId)
                .ToList();

            foreach (var transaction in transactionGroupedByCategoryId)
            {
                var categoryId = transaction.Key;
                var transactionsSum = transaction.Sum(x => x.Amount);
                var percentage = Math.Round((decimal)transactionsSum / (decimal)total, 2) * 100;
                var count = transaction.Count();

                yield return new TransactionImpact(categoryId, percentage, transactionsSum, count);
            }
        }

        private async Task<Result<IEnumerable<TransactionViewModel>>> GetTransactionsForPeriod(
            string userId, DateTime startDate, DateTime endDate)
        {
            var userResult = await _uow.UserRepository.GetUserWithTransactionsAsync(userId);

            if (userResult.IsFailed)
            {
                return Result.Fail("User not found".Using(_localizer));
            }

            var transactions = userResult.Value.Categories
                .SelectMany(c => c.Transactions)
                .Where(t => startDate <= t.Time && t.Time <= endDate)
                .ToList();

            return Result.Ok(_mapper.Map<IEnumerable<TransactionViewModel>>(transactions));
        }
    }
}
