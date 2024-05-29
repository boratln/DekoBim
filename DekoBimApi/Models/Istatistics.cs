using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class Istatistics
    {
        [Key]
        public int Id { get; set; }
        public int ViewCount { get; set; } = 0;
        public int DownloadCount { get; set; } = 0;
    }
}
