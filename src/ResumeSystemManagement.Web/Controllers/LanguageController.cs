using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ResumeSystemManagement.Web.Controllers;

public class LanguageController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetLanguage(string culture, string? returnUrl = null)
    {
        if (culture is not ("en-US" or "de-DE"))
            return BadRequest();

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                HttpOnly = true,
                Secure = true
            });

        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = Url.Action("Index", "Home");

        return LocalRedirect(returnUrl!);
    }
}
