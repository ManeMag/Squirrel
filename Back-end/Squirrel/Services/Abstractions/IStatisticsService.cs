using FluentResults;
using Squirrel.Responses.Statistics;

namespace Squirrel.Services.Abstractions
{
    public interface IStatisticsService
    {
        Task<Result<StatisticsViewModel>> GetStatisticsForHalfYear(string userId);
        Task<Result<StatisticsViewModel>> GetStatisticsForMonth(string userId);
        Task<Result<StatisticsViewModel>> GetStatisticsForPeriod(string userId, DateTime startDate, DateTime endDate);
    }
}