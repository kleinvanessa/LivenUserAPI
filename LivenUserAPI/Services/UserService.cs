using LivenUserAPI.Domain.Entities;
using LivenUserAPI.Repositories;

namespace LivenUserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task CreateUser(User user)
        {
            await _userRepository.AddNewUser(user);
        }       

        public async Task UpdateUser(User user)
        {
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if(user != null) 
            {
                await _userRepository.DeleteUserById(user);
            }
        }
    }
}
