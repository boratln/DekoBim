using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DekoBim.Controllers
{
    public class DisciplineController : Controller
    {
        private readonly HttpClient _client;
        private readonly INotyfService _notfy;

        Uri url = new Uri("http://45.136.6.177:1345/api");
        public DisciplineController(INotyfService notyf)
        {
            _client = new HttpClient();
            _client.BaseAddress = url;
           _notfy = notyf;

        }
        [HttpGet]
        public IActionResult AdminDisciplines()
        {
            List<DisciplineViewModel> disciplineViewModels = new List<DisciplineViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Discipline/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                disciplineViewModels=JsonConvert.DeserializeObject<List<DisciplineViewModel>>(data);
            }
            return View(disciplineViewModels);
        }
        [HttpPost]
        public IActionResult AdminDisciplines(DisciplineViewModel discipline)
        {
            var json = JsonConvert.SerializeObject(discipline);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Discipline/Post", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminDisciplines", "Discipline");
            }
            else
            {
                TempData["message"] = "Bir Hata Oluştu";
                return RedirectToAction("AdminDisciplines", "Discipline");

            }
        }
    }
}
