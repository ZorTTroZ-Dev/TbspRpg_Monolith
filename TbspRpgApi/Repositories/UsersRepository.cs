using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgApi.Repositories
{
    public interface IUsersRepository {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsernameAndPassword(string userName, string password);
    }
    
    public class UsersRepository : IUsersRepository
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

        public Task<User> GetUserByUsernameAndPassword(string userName, string password)
        {
            return _databaseContext.Users.AsQueryable().Where(user =>
                user.UserName == userName &&
                user.Password == password).FirstOrDefaultAsync();
        }
    }
}