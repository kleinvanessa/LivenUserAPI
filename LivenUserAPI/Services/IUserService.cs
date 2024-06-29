using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;

namespace LivenUserAPI.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateUser(LoginDTO loginDTO);
        Task<User> GetUserById(int id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
