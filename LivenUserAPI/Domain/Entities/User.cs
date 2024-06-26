﻿using System.ComponentModel.DataAnnotations;

namespace LivenUserAPI.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
