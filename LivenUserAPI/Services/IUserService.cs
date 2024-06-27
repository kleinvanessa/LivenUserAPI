using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
