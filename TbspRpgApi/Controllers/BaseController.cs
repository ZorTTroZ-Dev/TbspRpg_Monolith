using System;
using Microsoft.AspNetCore.Mvc;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected const string NotYourGameErrorMessage = "not your game";
        protected const string NotYourAdventureErrorMessage = "not your adventure";
        protected const string NotYourRouteErrorMessage = "not your route";

        protected Guid? GetUserId()
        {
            if(HttpContext != null)
                return (Guid?) HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY];
            return Guid.Empty;
        }
    }
}