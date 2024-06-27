using System.ComponentModel.DataAnnotations;

namespace LivenUserAPI.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
