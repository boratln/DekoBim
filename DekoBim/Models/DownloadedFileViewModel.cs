namespace DekoBim.Models
{
    public class DownloadedFileViewModel
    {
        public int Id { get; set; }
        public ProductsViewModel? product { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public int? DownloadCount { get; set; }
    }
}
