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
            //if (endDate > UtcNow) return Result.Fail("End date cannot be in future".Using(_localizer));

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
                Transactions = transactionsResult.Value,
                Impact = CalculateImpact(transactionsResult.Value),
                Income = transactionsResult.Value.Where(t => t.Amount > 0).Sum(t => t.Amount),
                Outcome = -transactionsResult.Value.Where(t => t.Amount < 0).Sum(t => t.Amount)
            });
        }

        public async Task<Result<StatisticsViewModel>> GetStatisticsForMonth(string userId) =>
            await GetStatisticsForPeriod(userId, MonthAgo, UtcNow);

        public async Task<Result<StatisticsViewModel>> GetStatisticsForHalfYear(string userId) =>
            await GetStatisticsForPeriod(userId, HalfYearAgo, UtcNow);

        private IEnumerable<TransactionImpact> CalculateImpact(IEnumerable<TransactionViewModel> transactions)
        {
            var totalNegative = transactions.Where(x => x.Amount < 0).Sum(t => t.Amount);
            var totalPositive = transactions.Where(x => x.Amount > 0).Sum(t => t.Amount);

            var transactionGroupedByCategoryId = transactions
                .GroupBy(t => t.CategoryId)
                .ToList();

            foreach (var transaction in transactionGroupedByCategoryId)
            {
                var categoryId = transaction.Key;
                var income = transaction.Where(x => x.Amount > 0).Sum(x => x.Amount);
                var outcome = transaction.Where(x => x.Amount < 0).Sum(x => x.Amount);
                var positivePercentage = totalPositive != 0 ? Math.Round((decimal)income / (decimal)totalPositive, 2) * 100 : 0;
                var negativePercentage = totalNegative != 0 ? Math.Round((decimal)outcome / (decimal)totalNegative, 2) * 100 : 0;
                var count = transaction.Count();

                yield return new TransactionImpact(categoryId, positivePercentage, income, negativePercentage, outcome, count);
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

            var transactions = userResult.Value.Categories!
                .SelectMany(c => c.Transactions)
                .Where(t => startDate <= t.Time && t.Time <= endDate)
                .ToList();

            return Result.Ok(_mapper.Map<IEnumerable<TransactionViewModel>>(transactions));
        }
    }
}
