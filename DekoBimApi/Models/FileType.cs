using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class FileType
    {
        [Key]
        public int Id { get; set; }
       
        public string? Name_ { get; set; }
    }
}
