namespace DekoBimApi.Models
{
    public class FileUpload
    {
        public IFormFile? Photo { get; set; }
      
        public IFormFile? revit { get; set; }
        public IFormFile? autocad { get; set; }
        public IFormFile? solidworks { get; set; }
        public IFormFile? ifcformat { get; set; }
       
        public List<IFormFile>? sartnameler { get; set; }
        public List<IFormFile>? sertifikalar { get; set; }
        public List<IFormFile>? kullanimkilavuz { get; set; }
        public List<IFormFile>? programlar { get; set; }
        public List<IFormFile>? digerdokumanlar { get; set; }
        public IFormFile? katalogfiyatliste { get; set; }

    }
}
