namespace DekoBim.Models
{
    public class ListModelView
    {
        public List<ProductCategoryViewModel>? Categories { get; set; }
        public List<DisciplineViewModel>? Disciplines { get; set; }
        public List<CompanyViewModel>? Companys { get; set; }
        public List<SubDisciplineViewModel>? SubDisciplines { get; set;}
        public List<FileTypesViewModel>? FileTypes { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<ProductsViewModel>? Products { get; set; }
        public int? usercount;
        public int? productcount;
        public int? ViewCount { get; set; }
        public int? DownloadCount { get; set; }
    }
}
