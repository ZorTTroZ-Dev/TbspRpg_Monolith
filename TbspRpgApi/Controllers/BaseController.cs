using System;
using Microsoft.AspNetCore.Mvc;
using TbspRpgApi.JwtAuthorization;

namespace TbspRpgApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected const string NotYourGameErrorMessage = "not your game";
        protected const string NotYourAdventureErrorMessage = "not your adventure";

        public BaseController() {}

        protected Guid? GetUserId()
        {
            return (Guid?) HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY];
        }

        protected bool CanAccessGame(Guid gameId)
        {
            return true;
        }

        protected bool CanAccessAdventure(Guid adventureId)
        {
            return true;
        }
    }
}