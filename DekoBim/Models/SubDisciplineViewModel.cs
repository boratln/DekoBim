using System.ComponentModel.DataAnnotations;
namespace DekoBim.Models
{
    public class SubDisciplineViewModel
    {
        public int Id { get; set; }
        public DisciplineViewModel discipline { get; set; } //disipline göre işlem yapacak
        [MaxLength(30)]
        public string? Name_ { get; set; }
    }
}
