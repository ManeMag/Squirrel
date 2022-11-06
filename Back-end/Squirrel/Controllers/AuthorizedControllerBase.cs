using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Squirrel.Entities;

namespace Squirrel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class AuthorizedControllerBase : Controller
    {
        protected AuthorizedControllerBase(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        protected UserManager<User> UserManager { get; set; }
        protected string GetUserId() => UserManager.GetUserId(User);
    }
}
