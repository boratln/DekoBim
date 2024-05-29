using System.ComponentModel.DataAnnotations;

namespace DekoBim.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [MaxLength(50)]

        public string? Name_ { get; set; }
        public string? Base64 { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyUrl { get; set; }
        public string? LinkedinUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstragramUrl { get; set; }
        [MaxLength(11)]
        public string? CompanyNumber { get; set; }

    }
}
