using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubDisciplinesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public SubDisciplinesController(RepositoryContext context)
        {
            _context = context;
        }
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var list=_context.SubDisciplines.Include(x=>x.discipline).ToList();
            return Ok(list);
        }
        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {

            var list = _context.SubDisciplines.Include(x => x.discipline).FirstOrDefault(x => x.Id == id);
            return Ok(list);
        }
        [HttpPost("Post")]
        public IActionResult Post(SubDiscipline? discipline)
        {
            var disiplin = _context.Disciplines.FirstOrDefault(x => x.Id == discipline.discipline.Id);
            if(disiplin == null)
            {
                return NotFound("Disiplin bulunamadı");
            }
            else
            {
                discipline.discipline = disiplin;
                _context.SubDisciplines.Add(discipline);
                _context.SaveChanges();
                return Ok("Başarıyla Eklendi");
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("ID not provided");
            }

            var subdiscipline = await _context.SubDisciplines.FirstOrDefaultAsync(x => x.Id == id);
            if (subdiscipline == null)
            {
                return NotFound("Subdiscipline not found");
            }

            var categories = _context.Categories.Where(p => p.subdicipline.Id == id).ToList();

            foreach (var category in categories)
            {
                var products = _context.Products.Where(p => p.Category.Id == category.Id).ToList();

                _context.Products.RemoveRange(products);

                _context.Categories.Remove(category);
            }

            // Delete the subdiscipline
            _context.SubDisciplines.Remove(subdiscipline);

            // Save all changes to the database
            await _context.SaveChangesAsync();

            return Ok("Subdiscipline deleted successfully");
        }

        [HttpPut("Update")]
        public async Task <IActionResult> Update(SubDiscipline subDiscipline)
        {
            if (subDiscipline.Id == null)
            {
                return NotFound("İd girilmedi");
            }
            var altdisiplin = await _context.SubDisciplines.FirstOrDefaultAsync(x => x.Id == subDiscipline.Id);

            if(altdisiplin== null)
            {
                return NotFound("Alt disiplin bulunamadı");
            }
            else
            {
                var disiplin = await _context.Disciplines.FirstOrDefaultAsync(x => x.Id == subDiscipline.discipline.Id);
                altdisiplin.discipline = disiplin;
                subDiscipline.discipline.Id = altdisiplin.discipline.Id;
                if (subDiscipline.Name_ != null)
                {
                    altdisiplin.Name_ = subDiscipline.Name_;

                }
                await _context.SaveChangesAsync();
                return Ok("Güncelleme işlemi başarılı");
            }
        }



        [HttpGet("Disiplin/{id}")]
        public IActionResult Disiplin(int id)
        {
            var disiplin = _context.SubDisciplines.Include(x => x.discipline).Where(x => x.discipline.Id == id).ToList();
            return Ok(disiplin);
        }

    }
}

