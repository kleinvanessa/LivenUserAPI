using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task AddNewUser(User user);
        Task UpdateUser(User user);
        Task DeleteUserById(User user);
        Task<User> GetUserByEmailAndPassword(string email, string password);
    }
}
