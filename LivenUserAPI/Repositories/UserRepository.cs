using LivenUserAPI.Data;
using LivenUserAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LivenUserAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;        
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddNewUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserById(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
        
    }
}
