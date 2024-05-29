using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public ProductCategoryController(RepositoryContext context)
        {
            _context = context;
        }
     
        [HttpGet("Get")]
        public async  Task<IActionResult> Get()
        {
            var list =  await _context.Categories.Include(x => x.subdicipline).Include(x => x.subdicipline.discipline).ToListAsync();
            return Ok(list);
        }
        [HttpGet("Get/{id}")]
        public IActionResult Get(int? id)
        {
            if (id == null)
            {
                return NotFound("id girilmeli");
            }
            var product = _context.Categories.
                Include(x => x.subdicipline)
                .Include(x => x.subdicipline.discipline).
                FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound("Ürün Bulunamadı");
            }
            else
            {
                return Ok(product);
            }
        }
        [HttpPost("Post")]
        public IActionResult Post(ProductCategory category)
        {
            var subdiscipline = _context.SubDisciplines.FirstOrDefault(x => x.Id == category.subdicipline.Id);
            if (subdiscipline == null)
            {
                return NotFound("Subdicipline bulunamadı.");
            }

            category.subdicipline = subdiscipline;
            if (category != null && category.teknikozellik != null && category.teknikozellikaralik!=null)
            {
                for (int i = 0; i < category.teknikozellik.Count; i++)
                {

                    if (i < category.teknikozellikaralik.Count)
                    {
                        string ozellikAdi = category.teknikozellik[i];
                        string mevcutDeger = category.teknikozellikaralik[i];

                        category.teknikozellikaralik[i] = ozellikAdi + ": " + mevcutDeger;
                    }
                }
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Ürün başarıyla eklendi.");
        }



        [HttpPost("yeniOzellikEkle")]
        public IActionResult yeniOzellikEkle(ProductCategory category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgisi eksik.");
            }

            var mevcutKategori = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (mevcutKategori == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            if (mevcutKategori.teknikozellik != null && category.teknikozellikaralik != null)
            {
                // mevcutKategori.teknikozellikaralik'ın başlatılması
                if (mevcutKategori.teknikozellikaralik == null)
                {
                    mevcutKategori.teknikozellikaralik = new List<string>();
                }

                // En küçük listeyi bulma
                int minUzunluk = Math.Min(mevcutKategori.teknikozellik.Count, category.teknikozellikaralik.Count);
                for (int i = 0; i < minUzunluk; i++)
                {
                    string birlesikDeger = mevcutKategori.teknikozellik[i] + ": " + category.teknikozellikaralik[i];
                    mevcutKategori.teknikozellikaralik.Add("! "+birlesikDeger);
                }
            }

            _context.SaveChanges();
            return Ok("Özellikler eklendi.");
        }
        [HttpGet("OzellikDegerleri/{categoryId}/{ozellikAdi}")]
        public ActionResult<IEnumerable<string>> GetOzellikDegerleri(int categoryId, string ozellikAdi)
        {
            var category = _context.Categories.Find(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            // Özellik adına göre teknik özellikler listesindeki ilgili değerleri filtreleyin
            var ozellikDegerleri = category.teknikozellikaralik?
                .Where(o => o.Contains(ozellikAdi))
                .Select(o => o.Split(':').Last()) // Varsayım: Değerler "ÖzellikAdı:Değer" formatında
                .Distinct()
                .ToList();

            return Ok(ozellikDegerleri);
        }


        [HttpGet("teknikozellik/{id}")]
        public async Task<IActionResult> teknikozellik(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound("Kategori bulunamadı");
            }
            else
            {
                return Ok(category.teknikozellik?.ToList());
            }
        }



        [HttpGet("teknikozellikcount/{id}")]
        public IActionResult teknikozellikcount(int? id)
        {
            if (id == null)
            {
                return NotFound("İd girilmedi");
            }
            var teknikozelliklist = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (teknikozelliklist?.teknikozellik?.Count == 0)
            {
                return NotFound("Teknik özellik yok");
            } 
            var count = teknikozelliklist?.teknikozellik?.Count();
            return Ok(count);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("İd girilmedi");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Kategori bulunamadı");
            }

            var products = _context.Products.Where(p => p.Category.Id == id);

            _context.Products.RemoveRange(products);

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return Ok("Kategori ve bağlı ürünler başarıyla silindi");
        }

        [HttpGet("AltDisiplin/{id}/{id2}")]
        public async Task<IActionResult> AltDisiplin(int? id, int? id2)
        {
            if (id == 0)
            {
                var altdisiplin = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == id2);

                // Null reference hatası burada oluşuyor olabilir
                var tumKategoriler = await _context.Categories
                    .Include(x => x.subdicipline)
                    .Include(x => x.subdicipline.discipline)
                    .Where(x => x.subdicipline.discipline.Id == altdisiplin.Id)
                    .ToListAsync();
                return Ok(tumKategoriler);
            }

            var kategori = await _context.Categories
                .Include(x => x.subdicipline)
                .Include(x => x.subdicipline.discipline)
                .Where(x => x.subdicipline.Id == id)
                .ToListAsync();

            return Ok(kategori);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductCategory category)
        {
            if (category.Id == null)
            {
                return NotFound("İd girilmedi"); 
            }

            var existingCategory = await _context.Categories.Include(c => c.subdicipline).FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existingCategory == null)
            {
                return NotFound("Kategori bulunamadı"); 
            }
            if (string.IsNullOrEmpty(category.Name_))
            {
                category.Name_ = existingCategory.Name_;
            }
            _context.Entry(existingCategory).CurrentValues.SetValues(category);

            if (category.subdicipline != null && category.subdicipline.Id != existingCategory.subdicipline?.Id)
            {
                var existingSubDiscipline = await _context.SubDisciplines.FindAsync(category.subdicipline.Id);
                if (existingSubDiscipline != null)
                {
                    existingCategory.subdicipline = existingSubDiscipline;
                }
                else
                {
                    return BadRequest("Hata");
                }
            }
            if (category.teknikozellik != null)
            {
                category.teknikozellik = existingCategory.teknikozellik;
            }

            await _context.SaveChangesAsync();
            return Ok("Güncelleme işlemi başarılı"); // Update successful
        }
        [HttpGet("Count/{id}")]
        public async Task<IActionResult> Count(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            var count = category?.teknikozellik?.Count();
            return Ok(count);
        }

    }
}
