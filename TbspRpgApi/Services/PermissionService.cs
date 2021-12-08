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
    }
    
    public class PermissionService: IPermissionService
    {
        private User User { get; set; }
        private HashSet<string> Permissions { get; set; }
        private readonly TbspRpgDataLayer.Services.IUsersService _usersService;
        private readonly TbspRpgDataLayer.Services.ILocationsService _locationsService;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            TbspRpgDataLayer.Services.IUsersService usersService,
            TbspRpgDataLayer.Services.ILocationsService locationsService,
            ILogger<PermissionService> logger)
        {
            _usersService = usersService;
            _locationsService = locationsService;
            _logger = logger;
        }
        
        private async Task LoadUser(Guid userId)
        {
            User ??= await _usersService.GetById(userId);
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
            var location = await _locationsService.GetLocationById(locationId);
            if (location == null)
                return false;

            return location.Adventure.CreatedByUserId == userId;
        }

        // they can access a location if they own the adventure that owns the route
        public async Task<bool> CanReadLocation(Guid userId, Guid locationId)
        {
            return await HasPermission(userId, 
                       TbspRpgSettings.Settings.Permissions.READ_LOCATION) || 
                   await CanAccessLocation(userId, locationId);
        }

        public async Task<bool> CanWriteLocation(Guid userId, Guid locationId)
        {
            return await HasPermission(userId,
                       TbspRpgSettings.Settings.Permissions.WRITE_LOCATION) || 
                   await CanAccessLocation(userId, locationId);
        }

        public async Task<bool> CanReadAdventure(Guid userId, Guid adventureId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanWriteAdventure(Guid userId, Guid adventureId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanReadGame(Guid userId, Guid gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanWriteGame(Guid userId, Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}