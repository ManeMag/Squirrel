using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        [HttpGet]
        public IActionResult GetCurrencies()
        {
            List<string> cultureList = new();
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => c.LCID);
            foreach (var culture in cultures)
            {
                RegionInfo region = new(culture);
                if (!cultureList.Contains(region.CurrencySymbol))
                    cultureList.Add(region.CurrencySymbol);
            }
            return base.Ok(cultureList.OrderBy(c => c.ToString()));
        }
    }
}
