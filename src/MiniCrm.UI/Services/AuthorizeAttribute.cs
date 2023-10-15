using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MiniCrm.UI.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string? Roles { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous) return;

        var role = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role");

        if (role is null)
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };

        if (!string.IsNullOrEmpty(Roles) && role is not null)
        {
            bool validate = false;

            string[] roles = Roles.Split(',');

            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == role.Value)
                {
                    validate = true;
                    break;
                }

                if (validate) break;
            }

            if (!validate)
                context.Result = new JsonResult(new { message = "Forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                };
        }
    }
}
