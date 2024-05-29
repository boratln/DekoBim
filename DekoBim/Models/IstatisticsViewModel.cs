namespace DekoBim.Models
{
    public class IstatisticsViewModel
    {
        public int Id { get; set; }
        public int? ViewCount { get; set; } = 0;
        public int? DownloadCount { get; set; } = 0;
    }
}
