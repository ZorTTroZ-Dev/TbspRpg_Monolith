using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgApi.Services;
using TbspRpgDataLayer.Tests;

namespace TbspRpgApi.Tests
{
    public class ApiTest
    {
        protected static UsersService CreateUsersService(IEnumerable<User> users)
        {
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            return new UsersService(dlUsersService);
        }
        
        protected static AdventuresService CreateAdventuresService(IEnumerable<Adventure> adventures)
        {
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            return new AdventuresService(dlAdventuresService);
        }
    }
}