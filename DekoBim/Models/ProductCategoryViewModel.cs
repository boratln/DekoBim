using System.ComponentModel.DataAnnotations.Schema;

namespace DekoBim.Models
{
    public class ProductCategoryViewModel
    {
        //ProductWebModel
        public int Id { get; set; }
        public SubDisciplineViewModel? subdicipline { get; set; } 
        public List<string>? teknikozellik { get; set; }
        public List<string>? teknikozellikaralik { get; set; }
        public string? Name_ { get; set; }
    }
}
