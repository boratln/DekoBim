using Newtonsoft.Json;

namespace DekoBim.Models
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public ProductCategoryViewModel? Category { get; set; }
        [JsonProperty("teknik_ozellikdata")]

        public List<string>? teknik_ozellikdata { get; set; }
        public CompanyViewModel? company { get; set; } //şirkete bağlı ürünler
        public FileTypesViewModel? FileType { get; set; }
        public string? Name_ { get; set; }
        public string? base64 { get; set; } //fotoğraflı ürünler
        public string? revit { get; set; }
        public string? revitversiyon { get; set; }
        public DateTime? revittarih { get; set; }
        public bool? IsExistRevit { get; set; } = false;

        public string? solidworks { get; set; }
        public string? solidversiyon { get; set; }
        public DateTime solidtarih { get; set; }
        public bool? isExistSolidworks { get; set; } = false;

        public string? autocad { get; set; }
        public string? autocadversiyon { get; set; }
        public DateTime autocadtarih { get; set; }
        public bool? isExistAutocad { get; set; } = false;

        public string? ifcformat { get; set; }
        public string? ifcversiyon { get; set; }
        public DateTime ifctarih { get; set; }
        public bool? Isexistifcformat { get; set; } = false;

        public string? description { get; set; }
        public List<string>? tanitimvideopath { get; set; }
        public List<string>? montajvideopath { get; set; }
        public int? Debi { get; set; }
        public int? basinc_kaybi { get; set; }
        public string? uygulama_alanlari { get; set; }
        public string? siniflandirma { get; set; }
        public string? diger { get; set; }
        public List<string>? sartnameler { get; set; }
        public List<string>? sertifakalar { get; set; }
        public List<string>? Kullanimkilavuzları { get; set; }
        public List<string>? Programlar { get; set; }
        public List<string>? DigerDokuman { get; set; }
        public List<string>? katalogfiyatliste { get; set; }


    }
}
