using System.Collections.Generic;
using System.Linq;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgApi.Tests
{
    public class ApiTest
    {
        protected static IUsersService MockDataLayerUsersService(IEnumerable<User> users)
        {
            var usersService = new Mock<IUsersService>();
            
            usersService.Setup(service =>
                service.Authenticate(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync((string userName, string password) =>
            {
                return users.FirstOrDefault(user => user.UserName == userName && user.Password == password);
            });

            return usersService.Object;
        }

        protected static IAdventuresService MockDataLayerAdventuresService(IEnumerable<Adventure> adventures)
        {
            var adventuresService = new Mock<IAdventuresService>();

            adventuresService.Setup(service =>
                    service.GetAllAdventures())
                .ReturnsAsync(adventures.ToList());
            
            adventuresService.Setup(service =>
                    service.GetAdventureByName(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                    adventures.FirstOrDefault(a => a.Name == name));

            return adventuresService.Object;
        }
        
        protected static TbspRpgApi.Services.UsersService CreateUsersService(IEnumerable<User> users)
        {
            var dlUsersService = MockDataLayerUsersService(users);
            return new TbspRpgApi.Services.UsersService(dlUsersService);
        }
        
        protected static TbspRpgApi.Services.AdventuresService CreateAdventuresService(IEnumerable<Adventure> adventures)
        {
            var dlAdventuresService = MockDataLayerAdventuresService(adventures);
            return new TbspRpgApi.Services.AdventuresService(dlAdventuresService);
        }
    }
}