using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DekoBim.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        private readonly INotyfService _notfy;
        Uri url = new Uri("http://45.136.6.177:1345/api");
        public UserController(INotyfService notfy)
        {
            _client = new HttpClient();
            _client.BaseAddress = url;
            _notfy = notfy;
        }
     
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdminPanel()
        {
            ListModelView list=new ListModelView();
            List<UserViewModel> users=new List<UserViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/User/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                users=JsonConvert.DeserializeObject<List<UserViewModel>>(data);
            }
          list.Users= users;
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress + "/User/Count").Result;
            if (response2.IsSuccessStatusCode)
            {
                var data= response2.Content.ReadAsStringAsync().Result;
                list.usercount=JsonConvert.DeserializeObject<int>(data);
            }
            HttpResponseMessage response3 = _client.GetAsync(_client.BaseAddress + "/Product/Count").Result;
            if (response3.IsSuccessStatusCode)
            {
                var data = response3.Content.ReadAsStringAsync().Result;
                list.productcount = JsonConvert.DeserializeObject<int>(data);
            }
            HttpResponseMessage response4 = _client.GetAsync(_client.BaseAddress + "/Istatistics/DownloadCount").Result;
            if (response4.IsSuccessStatusCode)
            {
                var data = response4.Content.ReadAsStringAsync().Result;
                list.DownloadCount = JsonConvert.DeserializeObject<int>(data);
            }
            HttpResponseMessage response5 = _client.GetAsync(_client.BaseAddress + "/Istatistics/ViewCount").Result;
            if (response5.IsSuccessStatusCode)
            {
                var data=response5.Content.ReadAsStringAsync().Result;
                list.ViewCount=JsonConvert.DeserializeObject<int>(data);
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Login(UserViewModel user)
        {
            var json = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/User/Login", content).Result;
            string data = response.Content.ReadAsStringAsync().Result;
            UserInfoViewModel kullanici = new UserInfoViewModel();

            if (response.IsSuccessStatusCode)
            {
                UserInfoViewModel userInfoFromApi = JsonConvert.DeserializeObject<UserInfoViewModel>(data);

                kullanici.Name = userInfoFromApi.Name;
                kullanici.Surname = userInfoFromApi.Surname;
                kullanici.Email = userInfoFromApi.Email;
                kullanici.Role = userInfoFromApi.Role;

                if (userInfoFromApi.Role == "Admin")
                { 
                    return RedirectToAction("AdminPanel", "User");
                }
               

                HttpContext.Session.SetString("ad", userInfoFromApi.Name);
                HttpContext.Session.SetString("soyad", userInfoFromApi.Surname);
                HttpContext.Session.SetString("rol", userInfoFromApi.Role);
                _notfy.Success("Başarıyla giriş yapıldı");
                return RedirectToAction("Filter", "Home");
            }
            else
            {

                TempData["error"] = "Email ya da şifre yanlış";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("sifre");
            HttpContext.Session.Remove("ad");
            HttpContext.Session.Remove("soyad");
            HttpContext.Session.Remove("rol");
            _notfy.Success("Başarıyla Çıkış Yapıldı", 3);
            return RedirectToAction("Filter", "Home");   
        }

        [HttpGet("Register")]

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register(UserViewModel user)
        {
            var json = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/User/Register", content).Result;
            string data = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Başarıyla kayıt olundu";
                return RedirectToAction("Login", "User");
            }
            else
            {
                TempData["error"] = "Email ya da şifre yanlış";
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadCountPage()
        {
            List<DownloadedFileViewModel> list=new List<DownloadedFileViewModel>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/DownloadedFile/Get");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                list=JsonConvert.DeserializeObject<List<DownloadedFileViewModel>>(data);    
            } 
            return View(list);
        }
    }
}
