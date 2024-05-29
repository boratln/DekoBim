using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanysController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public CompanysController(RepositoryContext repositoryContext)
        {
            _context = repositoryContext;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.Companys.ToListAsync();
            return Ok(list);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return NotFound("id girilmeli");
            }
            var company = await _context.Companys.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return NotFound("marka bulunamadı");
            }
            return Ok(company);
        }
        [HttpGet("CompanyProductCount/{id}")]
        public async Task<IActionResult> CompanyProductCount(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var count = await _context.Products.Where(x => x.company.Id == id).CountAsync();
            return Ok(count);
        }
        [HttpGet("MarkasiolanUrunler/{id}")]
        public async Task <IActionResult> MarkasiolanUrunler(int id)
        {
            var company = await _context.Companys.FirstOrDefaultAsync(x => x.Id == id);
            var list = await _context.Products.Where(x => x.company == company).Select(p => new
            {
                Id = p.Id,
                Base64 = p.base64,
                Name_ = p.Name_,
                sartnameler=p.sartnameler,
                sertifakalar = p.sertifakalar,
                Programlar=p.Programlar,
        
                teknik_ozellikdata = p.teknik_ozellikdata

            }).ToListAsync();                
            return Ok(list);
        }
        [HttpPost("Post")] 
        public IActionResult Post([FromForm] IFormFile? Photo, Company company)
        {
            if (Photo != null && Photo.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Photo.CopyTo(memoryStream);
                    byte[] photoBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(photoBytes);
                    company.Base64 = base64String;
                }
            }

            _context.Companys.Add(company);
            _context.SaveChanges();
            return Ok("Ürün başarıyla eklendi.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("id girilmedi");

            }
            var sirket = await _context.Companys.FirstOrDefaultAsync(x => x.Id == id);
            if (sirket == null)
            {
                return NotFound("Şirket bulunamadı");
            }
            else
            {
                var products = _context.Products.Where(x => x.company.Id == id);
                _context.Products.RemoveRange(products);
                _context.Companys.Remove(sirket);
               await _context.SaveChangesAsync();
                return Ok("Şirket başarıyla silindi");
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Company? companys)
        {
            if (companys == null)
            {
                return NotFound("İd girilmedi");
            }
            var sirket = await _context.Companys.FirstOrDefaultAsync(x => x.Id == companys.Id);
            if (sirket == null)
            {

                return NotFound("Disiplin bulunamadı");
            }
            else
            {
                if (companys.Name_ != null)
                {
                    sirket.Name_ = companys.Name_;
                }

                // Assume Base64 can also be null and only update if it's not null and not empty
                if (!string.IsNullOrEmpty(companys.Base64))
                {
                    sirket.Base64 = companys.Base64;
                }
               if (!string.IsNullOrEmpty(companys.CompanyAddress)){
                    sirket.CompanyAddress = companys.CompanyAddress;

                }
                if (!string.IsNullOrEmpty(companys.CompanyNumber))
                {
                    sirket.CompanyNumber = companys.CompanyNumber;

                }
                if (!string.IsNullOrEmpty(companys.CompanyEmail))
                {
                    sirket.CompanyEmail=companys.CompanyEmail;
                }
                if (!string.IsNullOrEmpty(companys.FacebookUrl))
                {
                    sirket.FacebookUrl = companys.FacebookUrl;

                }
                if (!string.IsNullOrEmpty(companys.LinkedinUrl))
                {
                    sirket.LinkedinUrl = companys.LinkedinUrl;

                }
                if (!string.IsNullOrEmpty(companys.CompanyDescription))
                {
                    sirket.CompanyDescription = companys.CompanyDescription;

                }
                if (!string.IsNullOrEmpty(companys.InstragramUrl))
                {
                    sirket.InstragramUrl = companys.InstragramUrl;

                }

                await _context.SaveChangesAsync();
                return Ok("Update successful.");
            }
        }
    }

}

