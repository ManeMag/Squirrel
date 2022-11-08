using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Squirrel.Responses.Statistics;
using Squirrel.Services.Abstractions;

namespace Squirrel.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public sealed class StatisticsController : AuthorizedControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(
            UserManager<User> userManager,
            IStatisticService statisticService) : base(userManager)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        /// Gets statistics for a specific period of time inclusive
        /// </summary>
        /// <param name="startDate">Start of the date time period to get statistics for</param>
        /// <param name="endDate">End of the date time period to get statistics for</param>
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

        /// <summary>
        /// Gets statistics for the last month - 30 days
        /// </summary>
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

        /// <summary>
        /// Gets statistics for the last half year - 180 days
        /// </summary>
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
