using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public DisciplineController(RepositoryContext context)
        {
            _context = context;
        }          
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var list=_context.Disciplines.ToList();
            return Ok(list);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                return NotFound("İd girilemedi");

            }
            else
            {
                var disiplin = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == id);
                if (disiplin == null)
                {
                    return NotFound("Disiplin bulunamadı");
                }
                else
                {
                    return Ok(disiplin);
                }
            }
        }
        [HttpPost("Post")]
        public IActionResult Post(Discipline discipline)
        {
            var disiplin=_context.Disciplines.FirstOrDefault(x=>x.Name_.ToUpper()==discipline.Name_.ToUpper());
            if (disiplin == null)
            {
                _context.Disciplines.Add(discipline);
                _context.SaveChanges();
                return Ok("Disiplin başarıyla kaydedildi");
            }
            else
            {
                return BadRequest(" Böyle bir Disiplin var");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("İd girilmedi");
            }

            var disiplin = await _context.Disciplines.FindAsync(id);
            if (disiplin == null)
            {
                return NotFound("Kategori bulunamadı");
            }

            var altdisiplinler = _context.SubDisciplines.Where(p => p.discipline.Id == id);
            var kategoriler = _context.Categories.Where(x => x.subdicipline.discipline.Id == id);
            var products = _context.Products.Where(x => x.Category.subdicipline.discipline.Id == id);

            _context.SubDisciplines.RemoveRange(altdisiplinler);
            _context.Products.RemoveRange(products);
            _context.Disciplines.Remove(disiplin);
            _context.Categories.RemoveRange(kategoriler);

            await _context.SaveChangesAsync();

            return Ok("Kategori ve bağlı ürünler başarıyla silindi");

        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Discipline disipline)
        {
            if (disipline == null)
            {
                return NotFound("İd girilmedi");
            }
            var disiplin = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == disipline.Id);
            if (disiplin == null)
            {

                return NotFound("Disiplin bulunamadı");
            }
            else
            {
               disiplin.Name_=disipline.Name_;
                _context.SaveChanges();
                return Ok("Güncelleme başarılı");
            }
        }

    }
}
