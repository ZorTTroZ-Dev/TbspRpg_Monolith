using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TbspRpgApi.JwtAuthorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public const string USER_ID_CONTEXT_KEY = "UserId";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = (string)context.HttpContext.Items[USER_ID_CONTEXT_KEY];
            if (userId == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}