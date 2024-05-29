using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class SubDiscipline
    {
        [Key]
        public int Id { get; set; } 
        public Discipline? discipline { get; set; } //disipline göre işlem yapacak
        [MaxLength(30)]
        public string? Name_ { get; set; }
    }
}
