using System.ComponentModel.DataAnnotations;

namespace DekoBim.Models
{
    public class DisciplineViewModel
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name_ { get; set; }
    }
}
