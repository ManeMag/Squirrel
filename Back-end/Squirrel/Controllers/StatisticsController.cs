using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Squirrel.Controllers
{
    public sealed class StatisticsController : AuthorizedControllerBase
    {
        public StatisticsController(
            UserManager<User> userManager) : base(userManager)
        {

        }
    }
}
