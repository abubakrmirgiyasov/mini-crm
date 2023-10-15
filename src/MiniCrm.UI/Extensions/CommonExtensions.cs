namespace MiniCrm.UI.Extensions;

public static class CommonExtensions
{
    public static IApplicationBuilder UseCustomJwt(this IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtMiddleware>();
    }

    public static void SetTokenCookie(this HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(1),
        };
        response.Cookies.Append("access_token", token, cookieOptions);
    }

    public static bool IsAuthenticated(this HttpContext context)
    {
        var type = context
            .User
            .Identities
            .Where(x => x.AuthenticationType is not null)
            .FirstOrDefault();

        return type is not null && type.IsAuthenticated;
    }

    public static bool IsInRole(this HttpContext context, string name)
    {
        var type = context
            .User
            .Identities
            .Where(x => x.AuthenticationType is not null)
            .FirstOrDefault();

        if (type is not null)
        {
            var role = type.FindFirst("role")?.Value;
            return role is not null && role == name;
        }

        return false;
    }
}
