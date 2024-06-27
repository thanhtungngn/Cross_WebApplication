using Cross_WebApplication.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cross_WebApplication.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        
        [Required]
        [Phone(ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
        [Required]
        public string Role { get; set; }

        public User CopyToUserEntity()
        {
            return new User { Id = Id, Name = Name, Surname = Surname, Email = Email, Phone = Phone, Role = Role };
        }
    }
}
