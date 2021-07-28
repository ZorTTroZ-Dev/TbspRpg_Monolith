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
    }
}