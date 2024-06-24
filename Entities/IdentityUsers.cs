using System.ComponentModel.DataAnnotations;

namespace Cross_WebApplication.Entities
{
    public class IdentityUsers
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }
    }
}
