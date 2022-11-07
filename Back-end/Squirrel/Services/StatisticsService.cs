using AutoMapper;
using FluentResults;
using Squirrel.Responses.Statistics;
using Squirrel.Responses.Transaction;
using Squirrel.Services.Repositories.Abstractions;
using static Squirrel.Responses.Statistics.StatisticsViewModel;

namespace Squirrel.Services
{
    public sealed class StatisticsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StatisticsService(
            IUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<StatisticsViewModel>> GetStatisticsForPeriod(
            string userId, DateTime startDate, DateTime endDate)
        {
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
                var percentage = (decimal)Math.Round(transactionsSum / total);
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
                return Result.Fail("User not found");
            }

            var transactions = userResult.Value.Categories
                .SelectMany(c => c.Transactions)
                .Where(t => startDate <= t.Time && t.Time <= endDate)
                .ToList();

            return Result.Ok(_mapper.Map<IEnumerable<TransactionViewModel>>(transactions));
        }

        private async Task<Result<IEnumerable<TransactionViewModel>>> GetTransactionsForLastMonth(string userId) =>
            await GetTransactionsForPeriod(userId, DateTime.UtcNow - TimeSpan.FromDays(30), DateTime.UtcNow);

        private async Task<Result<IEnumerable<TransactionViewModel>>> GetTransactionsForHalfYear(string userId) =>
            await GetTransactionsForPeriod(userId, DateTime.UtcNow - TimeSpan.FromDays(180), DateTime.UtcNow);
    }
}
