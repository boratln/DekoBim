using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string? Name_ { get; set; }    
    }
}
