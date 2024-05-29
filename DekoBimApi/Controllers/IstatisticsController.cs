using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IstatisticsController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public IstatisticsController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet("ViewCount")]
        public async Task <IActionResult> ViewCount()
        {
            var count = await _context.Istatistics.FirstOrDefaultAsync(x => x.Id == 1);
            return Ok(count.ViewCount);
        }
        [HttpGet("DownloadCount")]
        public async Task<IActionResult> DownloadCount()
        {
            var count=await _context.Istatistics.FirstOrDefaultAsync(x=>x.Id == 1);
            var downloadedfiles = _context.DownloadedFiles.Count();
            if (downloadedfiles == 0)
            {
                count.DownloadCount = 0;
            }
            return Ok(count.DownloadCount);
        }

        [HttpPost("IncreaseDownloadCount")]
        public async Task<IActionResult> IncreaseDownloadCount()
        {
            // İndirme sayısını artır
            var downloadCounter = await _context.Istatistics.FirstOrDefaultAsync(x=>x.Id==1);
            if (downloadCounter == null)
            {
                downloadCounter = new Istatistics { DownloadCount = 1 };
                _context.Istatistics.Add(downloadCounter);
            }
            else
            {
                downloadCounter.DownloadCount++;
            }

            await _context.SaveChangesAsync();

            return Ok(downloadCounter.DownloadCount);
        }
        [HttpPost("IncreaseViewCount")]
        public async Task<IActionResult> IncreaseViewCount()
        {
            // İndirme sayısını artır
            var ViewCounter = await _context.Istatistics.FirstOrDefaultAsync(x => x.Id == 1);
            if (ViewCounter == null)
            {
                ViewCounter = new Istatistics { DownloadCount = 1 };
                _context.Istatistics.Add(ViewCounter);
            }
            else
            {
                ViewCounter.ViewCount++;
            }

            await _context.SaveChangesAsync();

            return Ok(ViewCounter.ViewCount);
        }
    }
}
