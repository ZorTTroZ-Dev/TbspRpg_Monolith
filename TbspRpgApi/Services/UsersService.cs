using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgApi.Repositories;
using TbspRpgApi.Settings;

namespace TbspRpgApi.Services
{
    public interface IUsersService {
        Task<User> GetById(Guid id);
        Task<User> GetUserByUserNameAndPassword(string userName, string password);
    }
    
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IDatabaseSettings _databaseSettings;
        private readonly ILogger<UsersService> _logger;

        public UsersService(
            IUsersRepository usersRepository,
            IDatabaseSettings databaseSettings,
            ILogger<UsersService> logger)
        {
            _usersRepository = usersRepository;
            _databaseSettings = databaseSettings;
            _logger = logger;
        }

        public Task<User> GetById(Guid id)
        {
            return _usersRepository.GetUserById(id);
        }

        public Task<User> GetUserByUserNameAndPassword(string userName, string password)
        {
            // we'll need to add the salt and hash the password
            // then check that against the database value
            _logger.LogInformation("hashing password");
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(_databaseSettings.Salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
            _logger.LogInformation("password hashed");
            
            _logger.LogInformation("looking up user");
            return _usersRepository.GetUserByUsernameAndPassword(userName, hashedPassword);
        }
    }
}