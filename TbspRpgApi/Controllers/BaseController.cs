using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected const string NotYourGameErrorMessage = "not your game";
        protected const string NotYourAdventureErrorMessage = "not your adventure";
        protected const string NotYourRouteErrorMessage = "not your route";
        protected User RequestUser { get; set; }
        private readonly IUsersService _usersService;

        public BaseController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        protected Guid? GetUserId()
        {
            return (Guid?) HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY];
        }

        protected async Task LoadUser()
        {
            if (RequestUser == null)
            {
                // RequestUser = _usersService
            }
        }

        protected bool CanAccessGame(Guid gameId)
        {
            return true;
        }

        protected bool CanAccessAdventure(Guid adventureId)
        {
            return true;
        }

        protected bool CanAccessLocation(Guid locationId)
        {
            return true;
        }

        protected bool IsInGroup(string groupName)
        {
            return true;
        }
    }
}