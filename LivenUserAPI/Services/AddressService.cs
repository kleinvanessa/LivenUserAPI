using LivenUserAPI.Domain.Entities;
using LivenUserAPI.Repositories;

namespace LivenUserAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }               

        public async Task<Address> GetAddressById(int id)
        {
            return await _addressRepository.GetAddressById(id);
        }

        public async Task<IEnumerable<Address>> GetAllAddressesByUserId(int userId)
        {
            return await _addressRepository.GetAllAddressesByUserId(userId);
        }

        public async Task CreateAddress(Address address, int userId)
        {
            address.UserId = userId;

            await _addressRepository.AddNewAddress(address);
        }

        public async Task UpdateAddress(Address address)
        {
            await _addressRepository.UpdateAddress(address);
        }

        public async Task DeleteAddress(int id)
        {
            var address = await _addressRepository.GetAddressById(id);
            if (address != null) 
            {
                await _addressRepository.DeleteAddress(address);            
            }
        }
    }
}
