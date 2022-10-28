using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Squirrel.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(key: CookieRequestCultureProvider.DefaultCookieName,
                                    value: CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                                    options: new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return Ok();
        }
    }
}
