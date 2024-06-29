using LivenUserAPI.Domain.Entities;
using LivenUserAPI.Infrastructure.Data;
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
            var user = await _dbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);

            return user;
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

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _dbContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user;
        }
    }
}
