using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermission(Guid userId, string permissionName);
        Task<bool> IsInGroup(Guid userId, string groupName);
        Task<bool> CanReadLocation(Guid userId, Guid locationId);
        Task<bool> CanWriteLocation(Guid userId, Guid locationId);
        Task<bool> CanReadAdventure(Guid userId, Guid adventureId);
        Task<bool> CanWriteAdventure(Guid userId, Guid adventureId);
        Task<bool> CanReadGame(Guid userId, Guid gameId);
        Task<bool> CanWriteGame(Guid userId, Guid gameId);
        Task<bool> CanDeleteGame(Guid userId, Guid gameId);
        Task<bool> CanDeleteScript(Guid userId, Guid scriptId);
        Task<bool> CanDeleteRoute(Guid userId, Guid routeId);
    }
    
    public class PermissionService: IPermissionService
    {
        private User User { get; set; }
        private Adventure Adventure { get; set; }
        private Script Script { get; set; }
        private Route Route { get; set; }
        private Location Location { get; set; }
        private HashSet<string> Permissions { get; set; }
        private readonly TbspRpgDataLayer.Services.IUsersService _usersService;
        private readonly TbspRpgDataLayer.Services.ILocationsService _locationsService;
        private readonly TbspRpgDataLayer.Services.IAdventuresService _adventuresService;
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly TbspRpgDataLayer.Services.IScriptsService _scriptsService;
        private readonly TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            TbspRpgDataLayer.Services.IUsersService usersService,
            TbspRpgDataLayer.Services.ILocationsService locationsService,
            TbspRpgDataLayer.Services.IAdventuresService adventuresService,
            TbspRpgDataLayer.Services.IGamesService gamesService,
            TbspRpgDataLayer.Services.IScriptsService scriptsService,
            TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<PermissionService> logger)
        {
            _usersService = usersService;
            _locationsService = locationsService;
            _adventuresService = adventuresService;
            _gamesService = gamesService;
            _scriptsService = scriptsService;
            _routesService = routesService;
            _logger = logger;
        }
        
        private async Task LoadUser(Guid userId)
        {
            User ??= await _usersService.GetById(userId);
        }

        private async Task LoadAdventure(Guid adventureId)
        {
            Adventure ??= await _adventuresService.GetAdventureById(adventureId);
        }

        private async Task LoadScript(Guid scriptId)
        {
            Script ??= await _scriptsService.GetScriptById(scriptId);
        }

        private async Task LoadLocation(Guid locationId)
        {
            Location ??= await _locationsService.GetLocationById(locationId);
        }

        private async Task LoadRoute(Guid routeId)
        {
            Route ??= await _routesService.GetRouteById(routeId);
        }

        protected async Task LoadPermissions(Guid userId)
        {
            if (Permissions != null)
                return;
            
            await LoadUser(userId);
            Permissions = new HashSet<string>();
            foreach (var group in User.Groups)
            {
                foreach (var permission in group.Permissions)
                {
                    Permissions.Add(permission.Name);
                }
            }
        }

        public async Task<bool> HasPermission(Guid userId, string permissionName)
        {
            await LoadPermissions(userId);
            return Permissions.Contains(permissionName);
        }

        public async Task<bool> IsInGroup(Guid userId, string groupName)
        {
            await LoadUser(userId);
            return User.Groups.Any(group =>
                string.Equals(group.Name, groupName, StringComparison.CurrentCultureIgnoreCase));
        }

        private async Task<bool> CanAccessLocation(Guid userId, Guid locationId)
        {
            await LoadLocation(locationId);
            if (Location == null)
                return false;

            return Location.Adventure.CreatedByUserId == userId;
        }

        // they can access a location if they own the adventure that owns the location
        public async Task<bool> CanReadLocation(Guid userId, Guid locationId)
        {
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.ReadLocation) || 
                   await CanAccessLocation(userId, locationId);
        }

        public async Task<bool> CanWriteLocation(Guid userId, Guid locationId)
        {
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.WriteLocation)
                   || await IsInGroup(userId, TbspRpgSettings.Settings.Permissions.AdminGroup)
                   || await CanAccessLocation(userId, locationId);
        }

        private bool CanAccessAdventure(Guid userId)
        {
            if (Adventure == null)
                return false;

            return Adventure.CreatedByUserId == userId;
        }

        private bool IsAdventurePublished()
        {
            if (Adventure == null)
                return false;

            return DateTime.UtcNow >= Adventure.PublishDate;
        }

        public async Task<bool> CanReadAdventure(Guid userId, Guid adventureId)
        {
            await LoadAdventure(adventureId);
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.ReadAdventure)
                   || CanAccessAdventure(userId)
                   || IsAdventurePublished();
        }

        public async Task<bool> CanWriteAdventure(Guid userId, Guid adventureId)
        {
            await LoadAdventure(adventureId);
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.WriteAdventure)
                   || await IsInGroup(userId, TbspRpgSettings.Settings.Permissions.AdminGroup)
                   || CanAccessAdventure(userId);
        }

        private async Task<bool> CanAccessGame(Guid userId, Guid gameId)
        {
            var game = await _gamesService.GetGameById(gameId);
            if (game == null)
                return false;

            return game.UserId == userId;
        }

        public async Task<bool> CanReadGame(Guid userId, Guid gameId)
        {
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.ReadGame) ||
                   await CanAccessGame(userId, gameId);
        }

        public async Task<bool> CanWriteGame(Guid userId, Guid gameId)
        {
            return await HasPermission(userId, TbspRpgSettings.Settings.Permissions.WriteGame) ||
                   await CanAccessGame(userId, gameId);
        }

        public async Task<bool> CanDeleteGame(Guid userId, Guid gameId)
        {
            return await CanWriteGame(userId, gameId) ||
                   await IsInGroup(userId, TbspRpgSettings.Settings.Permissions.AdminGroup);
        }

        public async Task<bool> CanDeleteScript(Guid userId, Guid scriptId)
        {
            await LoadScript(scriptId);
            if (Script == null)
                return false;
            return await CanWriteAdventure(userId, Script.AdventureId);
        }

        public async Task<bool> CanDeleteRoute(Guid userId, Guid routeId)
        {
            await LoadRoute(routeId);
            if (Route == null)
                return false;
            return await CanWriteLocation(userId, Route.LocationId);
        }
    }
}