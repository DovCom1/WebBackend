using WebBackend.Model.Constants;

namespace WebBackend.Api.Service;

public static class CookieCreator
{
    public static void AddSidToCookie(string sid, HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddYears(1)
        };
        response.Cookies.Append(Constants.SidName, sid, cookieOptions);
    }
}