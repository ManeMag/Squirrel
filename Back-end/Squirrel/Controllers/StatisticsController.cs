using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Squirrel.Responses.Statistics;
using Squirrel.Services.Abstractions;

namespace Squirrel.Controllers
{
    public sealed class StatisticsController : AuthorizedControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(
            UserManager<User> userManager,
            IStatisticService statisticService) : base(userManager)
        {
            _statisticService = statisticService;
        }

        [HttpGet]
        public async Task<ActionResult<StatisticsViewModel>> GetStatistics(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var statisticResult = await _statisticService.GetStatisticsForPeriod(GetUserId(), startDate, endDate);

            if (statisticResult.IsFailed)
            {
                return BadRequest(statisticResult.Errors);
            }

            return Ok(statisticResult.Value);
        }

        [HttpGet("month")]
        public async Task<ActionResult<StatisticsViewModel>> GetStatisticsForMonth()
        {
            var statisticResult = await _statisticService.GetStatisticsForMonth(GetUserId());

            if (statisticResult.IsFailed)
            {
                return BadRequest(statisticResult.Errors);
            }

            return Ok(statisticResult.Value);
        }

        [HttpGet("half-year")]
        public async Task<ActionResult<StatisticsViewModel>> GetStatisticsForHalfYear()
        {
            var statisticResult = await _statisticService.GetStatisticsForHalfYear(GetUserId());

            if (statisticResult.IsFailed)
            {
                return BadRequest(statisticResult.Errors);
            }

            return Ok(statisticResult.Value);
        }
    }
}
