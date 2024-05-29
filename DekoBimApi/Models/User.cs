using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class User
    {
        [Key]

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; } = "";
    }
}
