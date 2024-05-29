using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public ProductController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products.Include(x => x.company).Include(x => x.Category).ThenInclude(x => x.subdicipline).ThenInclude(x => x.discipline).Select(p => new
            {
                Id = p.Id,
                Name_ = p.Name_,
                Category = p.Category,
                company = p.company,
                base64 = p.base64,
                discipline = p.Category.subdicipline.discipline,
                subdicipline = p.Category.subdicipline
            }).ToListAsync();
            return Ok(products);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Products.Include(x => x.Category).ToListAsync();
            return Ok(list);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Products urun)
        {
            if (urun.Id == null)
            {
                return NotFound("İd bulunamadı");
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == urun.Id);
            if (product == null)
            {
                return NotFound("Ürün bulunamadı");
            }

            product.Name_ = !string.IsNullOrEmpty(urun.Name_) ? urun.Name_ : product.Name_;
            product.description = !string.IsNullOrEmpty(urun.description) ? urun.description : product.description;
            product.diger = !string.IsNullOrEmpty(urun.diger) ? urun.diger : product.diger;
            product.siniflandirma = !string.IsNullOrEmpty(urun.siniflandirma) ? urun.siniflandirma : product.siniflandirma;
            product.uygulama_alanlari = !string.IsNullOrEmpty(urun.uygulama_alanlari) ? urun.uygulama_alanlari : product.uygulama_alanlari;
            product.Debi = urun.Debi ?? product.Debi;
            product.basinc_kaybi = urun.basinc_kaybi ?? product.basinc_kaybi;


            if (urun.base64 != null && urun.base64.Length > 0)
            {
                product.base64 = urun.base64;
            }

            if (urun.revit != null && urun.revit.Length > 0)
            {
                product.revit = urun.revit;
                product.revittarih = DateTime.Now;
                product.IsExistRevit = true;
            }

            if (urun.solidworks != null && urun.solidworks.Length > 0)
            {
                product.solidworks = urun.solidworks;
                product.solidtarih = DateTime.Now;
                product.isExistSolidworks = true;
            }

            if (urun.autocad != null && urun.autocad.Length > 0)
            {
                product.autocad = urun.autocad;
                product.autocadtarih = DateTime.Now;
                product.isExistAutocad = true;
            }

            if (urun.ifcformat != null && urun.ifcformat.Length > 0)
            {
                product.ifcformat = urun.ifcformat;
                product.ifctarih = DateTime.Now;
                product.Isexistifcformat = true;
            }

            if (urun.sartnameler != null && urun.sartnameler.Any())
            {
                product.sartnameler = urun.sartnameler;
            }
            if (urun.sertifakalar != null && urun.sertifakalar.Any())
            {
                product.sertifakalar = urun.sertifakalar;
            }
            if (urun.Programlar != null && urun.Programlar.Any())
            {
                product.Programlar = urun.Programlar;
            }
            if (urun.Kullanimkilavuzları != null && urun.Kullanimkilavuzları.Any())
            {
                product.Kullanimkilavuzları = urun.Kullanimkilavuzları;
            }
            if (urun.tanitimvideopath != null && urun.tanitimvideopath.Any())
            {
                product.tanitimvideopath = urun.tanitimvideopath;
            }
            if (urun.montajvideopath != null && urun.montajvideopath.Any())
            {
                product.montajvideopath = urun.montajvideopath;
            }
            if (urun.DigerDokuman != null && urun.DigerDokuman.Any())
            {
                product.DigerDokuman = urun.DigerDokuman;
            }
            if (urun.katalogfiyatliste != null && urun.katalogfiyatliste.Any())
            {
                product.katalogfiyatliste = urun.katalogfiyatliste;
            }
            if (urun.company != null && urun.company.Id > 0)
            {
                var company = await _context.Companys.FindAsync(urun.company.Id);
                if (company != null)
                {
                    product.company = company;
                }
            }
            if (urun.Category != null && urun.Category.Id > 0)
            {
                var category = await _context.Categories.FindAsync(urun.Category.Id);
                if (category != null)
                {
                    product.Category = category;
                }
            }
            if (urun.teknik_ozellikdata != null && urun.teknik_ozellikdata.Any())
            {
                product.teknik_ozellikdata = urun.teknik_ozellikdata;

            }
            await _context.SaveChangesAsync();
            return Ok("Güncelleme işlemi başarıyla yapıldı");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("İd girilmedi");
            }



            var products = _context.Products.FirstOrDefault(p => p.Id == id);
            var downloadfiles = _context.DownloadedFiles.Where(x => x.product.Id == products.Id);
            if (products == null)
            {
                return NotFound("Ürün bulunamadı");
            }
            else
            {
                _context.DownloadedFiles.RemoveRange(downloadfiles);
                _context.Products.Remove(products);
                await _context.SaveChangesAsync();
                return Ok(" ürünler başarıyla silindi");
            }

        }

        [HttpGet("Count")]
        public IActionResult Count()
        {
            var count = _context.Products.Count();
            return Ok(count);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.Include(x => x.company)
         .Select(p => new
         {
             Id = p.Id,
             Name_ = p.Name_,
             base64 = p.base64,
             revit = p.revit,
             teknik_ozellikdata = p.teknik_ozellikdata,
             solidworks = p.solidworks,
             autocad = p.autocad,
             ifcformat = p.ifcformat,
             description = p.description,
             Company = p.company,
             Revitversiyon = p.revitversiyon,
             Autocadversiyon = p.autocadversiyon,
             Solidversiyon = p.solidversiyon,
             Ifcversiyon = p.ifcversiyon,
             uygulama_alanlari = p.uygulama_alanlari,
             siniflandirma = p.siniflandirma,
             diger = p.diger,
             sartnameler = p.sartnameler,
             sertifakalar = p.sertifakalar,
             DigerDokuman = p.DigerDokuman,
             programlar = p.Programlar,
             katalogfiyatliste = p.katalogfiyatliste,
             revittarih = p.revittarih,
             solidworkstarih = p.solidtarih,
             autocadtarih = p.autocadtarih,
             ifcformattarih = p.ifctarih,
             montajvideopath = p.montajvideopath,
             tanitimvideopath = p.tanitimvideopath,
             Kullanimkilavuzları = p.Kullanimkilavuzları
         })
         .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromForm] FileUpload dosya, Products product)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == product.Category.Id);
            var company = await _context.Companys.FirstOrDefaultAsync(x => x.Id == product.company.Id);
            if (dosya.Photo != null && dosya.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    await dosya.Photo.CopyToAsync(ms);
                    photoBytes = ms.ToArray();
                    product.base64 = Convert.ToBase64String(photoBytes);
                }
            }
            if (dosya.revit != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] revitbytes;
                    await dosya.revit.CopyToAsync(ms);
                    revitbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(revitbytes);
                    product.revit = base64;
                    product.revittarih = DateTime.Now;
                }
            }
            if (dosya.solidworks != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] solidbytes;
                    await dosya.solidworks.CopyToAsync(ms);
                    solidbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(solidbytes);

                    product.solidworks = base64;
                    product.solidtarih = DateTime.Now;
                }
            }
            if (dosya.autocad != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] autocadbytes;
                    await dosya.autocad.CopyToAsync(ms);
                    autocadbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(autocadbytes);

                    product.autocad = base64;
                    product.autocadtarih = DateTime.Now;
                }
            }
            if (dosya.ifcformat != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] ifcbytes;
                    await dosya.ifcformat.CopyToAsync(ms);
                    ifcbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(ifcbytes);

                    product.ifcformat = base64;
                    product.ifctarih = DateTime.Now;
                }
            }
            if (dosya.sartnameler != null && dosya.sartnameler.Any())
            {
                product.sartnameler = new List<string>();
                foreach (var dosyas in dosya.sartnameler)
                {
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var dosyabytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(dosyabytes);

                            product.sartnameler.Add(base64);
                        }
                    }
                }
            }
            if (dosya.sertifikalar != null && dosya.sertifikalar.Any())
            {
                product.sertifakalar = new List<string>();
                foreach (var dosyas in dosya.sertifikalar)
                {
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);

                            product.sertifakalar.Add(base64);
                        }
                    }
                }
            }

            if (dosya.kullanimkilavuz != null && dosya.kullanimkilavuz.Any())
            {
                product.Kullanimkilavuzları = new List<string>();
                foreach (var dosyas in dosya.kullanimkilavuz)
                {
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Kullanimkilavuzları.Add(base64);
                        }
                    }
                }
            }
            if (dosya.programlar != null && dosya.programlar.Any())
            {
                product.Programlar = new List<string>();
                foreach (var dosyas in dosya.programlar)
                {
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Programlar.Add(base64);
                        }
                    }
                }
            }
            if (dosya.digerdokumanlar != null && dosya.digerdokumanlar.Any())
            {
                product.DigerDokuman = new List<string>();
                foreach (var dosyas in dosya.digerdokumanlar)
                {
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.DigerDokuman.Add(base64);
                        }
                    }
                }
            }
            if (category != null && category.teknikozellik != null && product.teknik_ozellikdata != null)
            {
                for (int i = 0; i < category.teknikozellik.Count; i++)
                {

                    if (i < product.teknik_ozellikdata.Count)
                    {
                        string ozellikAdi = category.teknikozellik[i];
                        string mevcutDeger = product.teknik_ozellikdata[i];

                        product.teknik_ozellikdata[i] = ozellikAdi + ": " + mevcutDeger;
                    }
                }
            }

            product.Category = category;
            product.company = company;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return Ok("Ürün Başarıyla Eklendi");
        }
        [HttpPost("Filter")]
        public async Task<IActionResult> Filter(Products product)
        {
            int? categoryId = product.Category?.Id; 
            int? subDisciplineId = product.Category.subdicipline?.Id; 
            int? disciplineId = product.Category.subdicipline?.discipline?.Id;
            int? companyId = product.company?.Id; 
            int? FileTypeId = product.FileType?.Id;
            bool? revit = product.IsExistRevit;
            bool? autocad = product.isExistAutocad;
            bool? solidworks = product.isExistSolidworks;
            bool? ifcformat = product.Isexistifcformat;
            var query = _context.Products.AsQueryable();
            if (companyId.HasValue && companyId.Value > 0)
            {
                query = query.Where(x => x.company.Id == companyId.Value);
            }
            if(disciplineId.HasValue && disciplineId.Value > 0)
            {
                query = query.Where(x => x.Category.subdicipline.discipline.Id == disciplineId.Value);
            }
            // Filter by category if not "All"
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(x => x.Category.Id == categoryId.Value);
            }

            // Filter by sub-discipline if not "All"
            if (subDisciplineId.HasValue && subDisciplineId.Value > 0)
            {
                query = query.Where(x => x.Category.subdicipline.Id == subDisciplineId.Value);
            }
            // Filter by discipline if not "All"
            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                query = query.Where(x => x.Category.subdicipline.discipline.Id == disciplineId.Value);
            }

            if (FileTypeId.HasValue && FileTypeId.Value == 1)
            {
                query = query.Where(x => x.isExistSolidworks == true);
            }
            if (FileTypeId.HasValue && FileTypeId.Value == 2)
            {
                query = query.Where(x => x.Isexistifcformat == true);
            }

            if (FileTypeId.HasValue && FileTypeId.Value == 4)
            {
                query = query.Where(x => x.IsExistRevit == true);
            }
            if (FileTypeId.HasValue && FileTypeId.Value == 5)
            {
                query = query.Where(x => x.isExistAutocad == true);
            }
            var filteredProducts = await query
      .Include(p => p.Category)
          .ThenInclude(c => c.subdicipline)
              .ThenInclude(sd => sd.discipline)
      .Select(p => new
      {
          Id = p.Id,
          Base64 = p.base64,
          category = p.Category,
          subdicipline = p.Category.subdicipline,
          discipline = p.Category.subdicipline.discipline,
          description = p.description,
          teknik_ozellikdata = p.teknik_ozellikdata,
          company = p.company,
          name_ = p.Name_,
          debi = p.Debi,
          Basinc_kaybi = p.basinc_kaybi
      })
      .ToListAsync();


            if (filteredProducts.Any())
            {
                return Ok(filteredProducts);
            }

            return BadRequest("Filtrelemede sorun çıktı");
        }

        [HttpGet("SearchByFeatures")]
        public IActionResult SearchByFeatures(int categoryId, [FromQuery(Name = "features")] Dictionary<string, string> features)
        {
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.company)
                .Where(p => p.Category.Id == categoryId)
                .ToList();

            var updatedProducts = productsQuery
        .Where(p => p.teknik_ozellikdata != null && p.teknik_ozellikdata.All(ozellik =>
        {
            var parts = ozellik.Split(':', 2);
            if (parts.Length != 2) return false;

            var featureKey = parts[0].Trim();
            var featureValue = parts[1].Trim();

            if (!features.ContainsKey(featureKey)) return false;

            var featureRange = features[featureKey].Split('-');
            if (featureRange.Length != 2 ||
                !int.TryParse(featureRange[0], out int minRange) ||
                !int.TryParse(featureRange[1], out int maxRange))
            {
                return false;
            }

            // Sayısal değerleri metinsel içerikten ayır
            var matches = Regex.Matches(featureValue, @"\d+");
            var valueParts = matches.Cast<Match>().Select(m => m.Value).ToArray();

            if (valueParts.Length >= 1 &&
                int.TryParse(valueParts[0], out int singleValue) &&
                singleValue >= minRange && singleValue <= maxRange)
            {
                // Eğer ilk sayısal değer aralıkta ise, true dön
                return true;
            }

            return false;
        }))
        .ToList()

       .Select(p => new Products
       {
           // Ürün bilgilerini ayarla
           Id = p.Id,
           Category = p.Category,
           company = p.company,
           Name_ = p.Name_,
           base64 = p.base64,
           teknik_ozellikdata = p.teknik_ozellikdata // Burada güncelleme yapmanıza gerek kalmayabilir
       })
       .ToList();

            if (updatedProducts.Count==0)
            {
                return Ok(productsQuery);
            }
            else
            {
                return Ok(updatedProducts);

            }
        }





            [HttpGet("OzellikAd/{id}")]
            public async Task<IActionResult> OzellikAd(int id)
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


        [HttpGet("Markalistesartnameurun/{id}/{id2}/{id3}")]
        public async Task<IActionResult> Markalistesartnameurun(int id,int id2,int id3)
        {
            var disiplin = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == id);
            if (disiplin == null)
            {
                return NotFound("Disiplin bulunamadı");
            }
           
            var altdisiplin =  _context.SubDisciplines.Where(x => x.Id == id2);
            if (altdisiplin == null)
            {
                return NotFound("Alt disiplin bulunamadı");
            }
            var kategori = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id3);
            if (kategori == null)
            {
                return NotFound("Kategori bulunamadı");
            }
            var filtrelenecekurunler =  _context.Products.Include(x=>x.company).Include(x=>x.Category).Include(x=>x.Category.subdicipline.discipline).Include(x=>x.Category.subdicipline)
                .Where(x => x.Category.Id == kategori.Id && x.Category.subdicipline.Id==id2 && x.Category.subdicipline.discipline.Id==id).ToList();
            if (filtrelenecekurunler.Count == 0)
            {
                return NotFound("Ürün bulunamadı");
            }
            else
            {
                return Ok(filtrelenecekurunler);
            }
        }

        [HttpGet("markasartnamebilgigetir/{id}")]
        public async Task<IActionResult> markasartnamebilgigetir(int id)
        {
            var product = await _context.Products.Include(x=>x.company).Include(x=>x.Category).ThenInclude(x=>x.subdicipline).ThenInclude(x=>x.discipline).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) { return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }


















    }
    }

