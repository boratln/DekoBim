using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekoBimApi.Models
{
    public class ProductCategory
    {
        //Api Model
        [Key]
        public int Id { get; set; }
        public SubDiscipline? subdicipline { get; set; } //alt disipline bağlı ürünler
        public List<string>? teknikozellik { get; set; }
        public List<string>? teknikozellikaralik { get; set; }
        public string? Name_ { get; set; }
    }
}
