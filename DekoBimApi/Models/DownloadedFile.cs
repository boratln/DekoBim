using System.ComponentModel.DataAnnotations;

namespace DekoBimApi.Models
{
    public class DownloadedFile
    {
        [Key]
        public int Id { get; set; }
        public Products? product { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public int? DownloadCount { get; set; }
    }
}
