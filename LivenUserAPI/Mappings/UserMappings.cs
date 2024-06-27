using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;

namespace LivenUserAPI.Mappings
{
    public static class UserMappings
    {
        public static User ToDomain(this UserDTO userDTO)
        {
            return new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Addresses = userDTO.Addresses?.Select(a => a.ToDomain()).ToList()
            };
        }
    }
}
