using Microsoft.AspNetCore.Mvc;
using Squirrel.Contexts;

namespace Squirrel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            this.context = context;
        }
    }
}