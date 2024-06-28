using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Services
{
    public interface IAddressService
    {
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Address>> GetAllAddressesByUserId(int userId);
        Task CreateAddress(Address address, int userId);
        Task UpdateAddress(Address address);
        Task DeleteAddress(int id);
        Task<bool> VerifyAddressByUserId(int addressId, int userId);
    }
}
