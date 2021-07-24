using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgApi.Repositories
{
    public interface IUserRepository {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsernameAndPassword(string username, string password);
    }
    
    public class UsersRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UsersRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public Task<User> GetUserById(Guid id)
        {
            return _databaseContext.Users.AsQueryable().Where(user => user.Id == id).FirstOrDefaultAsync();
        }

        public Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return _databaseContext.Users.AsQueryable().Where(user =>
                user.UserName == username &&
                user.Password == password).FirstOrDefaultAsync();
        }
    }
}