using FluentResults;
using Squirrel.Responses.Statistics;

namespace Squirrel.Services.Abstractions
{
    public interface IStatisticService
    {
        Task<Result<StatisticsViewModel>> GetStatisticsForHalfYear(string userId);
        Task<Result<StatisticsViewModel>> GetStatisticsForMonth(string userId);
        Task<Result<StatisticsViewModel>> GetStatisticsForPeriod(string userId, DateTime startDate, DateTime endDate);
    }
}