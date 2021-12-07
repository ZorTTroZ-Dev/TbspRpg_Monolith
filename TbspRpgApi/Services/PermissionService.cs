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
        Task<bool> CanAccessLocation(Guid userId, Guid locationId);
        Task<bool> CanAccessAdventure(Guid userId, Guid adventureId);
        Task<bool> CanAccessGame(Guid userId, Guid gameId);
    }
    
    public class PermissionService: IPermissionService
    {
        private User User { get; set; }
        private HashSet<string> Permissions { get; set; }
        private readonly IUsersService _usersService;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IUsersService usersService, ILogger<PermissionService> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }
        
        private async Task LoadUser(Guid userId)
        {
            User ??= await _usersService.GetUserEntityById(userId);
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

        public async Task<bool> CanAccessLocation(Guid userId, Guid locationId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanAccessAdventure(Guid userId, Guid adventureId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanAccessGame(Guid userId, Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}