using System;
using Microsoft.AspNetCore.Mvc;
using TbspRpgApi.JwtAuthorization;

namespace TbspRpgApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController() {}

        public Guid? GetUserId()
        {
            return (Guid?) HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY];
        }
    }
}