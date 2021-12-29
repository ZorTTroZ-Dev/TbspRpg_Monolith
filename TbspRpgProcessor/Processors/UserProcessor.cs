using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IUserProcessor
    {
        Task<User> RegisterUser(UserRegisterModel userRegisterModel);
        Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel);
    }
    
    public class UserProcessor : IUserProcessor
    {
        private readonly IUsersService _usersService;
        private readonly IMailClient _mailClient;
        private readonly ILogger<UserProcessor> _logger;

        public UserProcessor(IUsersService usersService,
            IMailClient mailClient,
            ILogger<UserProcessor> logger)
        {
            _usersService = usersService;
            _mailClient = mailClient;
            _logger = logger;
        }

        public async Task<User> RegisterUser(UserRegisterModel userRegisterModel)
        {
            var dbUser = await _usersService.GetUserByEmail(userRegisterModel.Email);
            if (dbUser != null)
                throw new ArgumentException("email already exists");

            var randomNumber = new Random();
            var registrationKeyInt = randomNumber.Next(1000000);
            var registrationKey = registrationKeyInt.ToString("000000");
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = userRegisterModel.Email,
                Password = _usersService.HashPassword(userRegisterModel.Password),
                RegistrationKey = registrationKey,
                RegistrationComplete = false,
                DateCreated = DateTime.UtcNow
            };
            await _usersService.AddUser(user);
            await _usersService.SaveChanges();
            
            // await _mailClient.SendRegistrationVerificationMail(user.Email, user.RegistrationKey);
            
            return user;
        }

        public async Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel)
        {
            var dbUser = await _usersService.GetById(userVerifyRegisterModel.UserId);
            if (dbUser == null)
                throw new ArgumentException("invalid user id");

            if (dbUser.RegistrationComplete)
                throw new Exception("registration already complete");

            if (dbUser.RegistrationKey != userVerifyRegisterModel.RegistrationKey)
                return null;

            dbUser.RegistrationComplete = true;
            dbUser.RegistrationKey = null;
            await _usersService.SaveChanges();
            return dbUser;
        }
    }
}