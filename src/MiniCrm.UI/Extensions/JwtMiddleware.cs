using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniCrm.UI.Extensions;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("access_token", out string? accessToken))
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Request.Headers.Add("Authorization", accessToken);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken.Split(" ").Last());

                var claims = new ClaimsIdentity(token.Claims, "custom");
                context.User.AddIdentity(claims);
            }
        }

        return _next(context);
    }
}
