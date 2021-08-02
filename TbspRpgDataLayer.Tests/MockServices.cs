using System.Collections.Generic;
using System.Linq;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgDataLayer.Tests
{
    public static class MockServices
    {
        public static IUsersService MockDataLayerUsersService(IEnumerable<User> users)
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

        public static IAdventuresService MockDataLayerAdventuresService(IEnumerable<Adventure> adventures)
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
    }
}