using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Squirrel.Extensions
{
    public static class StringExtensions
    {
        public static string Using(this string @this, IStringLocalizer localizer) =>
            localizer[@this].Value;

        public static BadRequestObjectResult ToBadRequestUsing(this string @this, IStringLocalizer localizer) =>
            new BadRequestObjectResult(new[] { @this.Using(localizer) });
    }
}
