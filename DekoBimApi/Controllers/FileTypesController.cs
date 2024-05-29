using DekoBimApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileTypesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public FileTypesController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet("Get")]
        public async Task <IActionResult> Get()
        {
            var list= await _context.FileTypes.ToListAsync();
            return Ok(list);
        }
    }
}
