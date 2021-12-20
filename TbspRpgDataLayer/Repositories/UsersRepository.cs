using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IUsersRepository {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByEmailAndPassword(string email, string password);
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
            return _databaseContext.Users.AsQueryable()
                .Where(user => user.Id == id)
                .Include(user => user.Groups)
                .ThenInclude(group => group.Permissions)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return _databaseContext.Users.AsQueryable().Where(user =>
                user.Email == email &&
                user.Password == password).FirstOrDefaultAsync();
        }
    }
}