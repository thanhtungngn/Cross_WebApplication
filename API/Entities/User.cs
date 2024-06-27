using System.ComponentModel.DataAnnotations;

namespace Cross_WebApplication.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }
    }
}
