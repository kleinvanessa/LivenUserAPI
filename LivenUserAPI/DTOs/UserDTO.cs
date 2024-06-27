namespace LivenUserAPI.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<AddressDTO> Addresses { get; set; }
    }
}
