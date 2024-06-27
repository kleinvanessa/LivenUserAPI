using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Address>> GetAllAddressesByUserId(int userId);
        Task CreateAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(int id);
    }
}
