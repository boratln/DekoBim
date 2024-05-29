using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadedFileController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public DownloadedFileController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpPost("Post")]
        public async Task<IActionResult> Post(DownloadedFile downloadedFile)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == downloadedFile.product.Id);
            if (product != null)
            {
                downloadedFile.product = product;
            }
            else
            {
                return NotFound("Ürün bulunamadı.");
            }

            var dbFile = await _context.DownloadedFiles
                                       .FirstOrDefaultAsync(x => x.product.Id == downloadedFile.product.Id &&
                                                                 x.FileName == downloadedFile.FileName);
            if (dbFile == null)
            {
                // Yeni kayıt ekle
                downloadedFile.DownloadCount = 1; // Yeni dosya olduğu için indirme sayısını 1 olarak başlat
                _context.DownloadedFiles.Add(downloadedFile);
            }
            else
            {
                // Mevcut kaydı güncelle
                dbFile.DownloadCount++;
                // Gerekiyorsa diğer alanları da güncelleyin
            }

            await _context.SaveChangesAsync();
            return Ok(dbFile == null ? "Dbye eklendi" : "Güncelleme İşlemi başarılı");
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.DownloadedFiles.Include(x=>x.product.company).ToListAsync();
            return Ok(list);
        }
    }
}
