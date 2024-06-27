using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;

namespace LivenUserAPI.Mappings
{
    public static class AddressMappings
    {
        public static Address ToDomain(this AddressDTO addressDTO)
        {
            return new Address
            {
                Street = addressDTO.Street,
                City = addressDTO.City,
                Country = addressDTO.Country,
                PostalCode = addressDTO.PostalCode
            };
        }
    }
}
