using LivenUserAPI.Domain.Entities;

namespace LivenUserAPI.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Address>> GetAllAddressesByUserId(int userId);
        Task AddNewAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(Address address);
        Task<bool> VerifyAddressByUserId(int addressId, int userId);
    }
}
