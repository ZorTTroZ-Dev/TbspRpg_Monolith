using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;

namespace TbspRpgDataLayer.Services
{
    public interface IUsersService {
        Task<User> GetById(Guid id);
        Task<User> Authenticate(string userName, string password);
        string HashPassword(string password);
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

        public Task<User> Authenticate(string userName, string password)
        {
            var hashedPassword = HashPassword(password);
            return GetUserByUserNameAndPassword(userName, hashedPassword);
        }

        public string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(_databaseSettings.Salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
        }

        public Task<User> GetUserByUserNameAndPassword(string userName, string password)
        {
            return _usersRepository.GetUserByUsernameAndPassword(userName, password);
        }
    }
}