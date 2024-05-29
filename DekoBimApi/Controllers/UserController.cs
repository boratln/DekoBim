using DekoBimApi.Data;
using DekoBimApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekoBimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public UserController(RepositoryContext context)
        {
            _context= context;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            var list = _context.Users.Where(x => x.Rol == "User").ToList();
            return Ok(list);
        }
        [HttpGet("Count")]
        public IActionResult Count() {
        var count=_context.Users.Where(x=>x.Rol=="User").Count();
            return Ok(count);
        }
        [HttpPost("Register")]
        public async Task <IActionResult> Register(User user)
        {
            if (user == null)
            {
                return NotFound("Veri girilmemiş");
            }
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email && x.Password == user.Password);
            if (User != null)
            {
                return BadRequest("Böyle bir kullanıcı var!");
            }
            else
            {
                user.Rol = "User";
               await _context.Users.AddAsync(user);
               await _context.SaveChangesAsync();
                return Ok("Başarıyla Kayıt Olundu");
            }
        }
        [HttpPost("Login")]
        public async Task <IActionResult> Login(User user)
        {
            if (user == null)
            {
                return BadRequest("input girilmedi");
            }
            else
            {
                var loginUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email && x.Password == user.Password);
                if (loginUser != null)
                {
                    UserInfo kullanici = new UserInfo
                    {
                        Name = loginUser.Name,
                        Surname = loginUser.Surname,
                        Email = loginUser.Email,
                        Role = loginUser.Rol
                    };

                    return Ok(kullanici);
                }
                else
                {
                    return NotFound("Kullanıcı Bulunamadı");
                }
            }
        }

    }
}
