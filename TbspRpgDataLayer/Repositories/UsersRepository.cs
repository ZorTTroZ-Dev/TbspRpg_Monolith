using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IUsersRepository: IBaseRepository {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<User> GetUserByEmail(string email);
        Task AddUser(User user);
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
                user.Email.ToLower() == email.ToLower() &&
                user.Password == password)
                .Include(user => user.Groups)
                .ThenInclude(group => group.Permissions)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserByEmail(string email)
        {
            return _databaseContext.Users.AsQueryable().FirstOrDefaultAsync(
                user => user.Email.ToLower() == email.ToLower());
        }

        public async Task AddUser(User user)
        {
            await _databaseContext.Users.AddAsync(user);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}