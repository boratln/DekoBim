using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DekoBim.Controllers
{
    public class SubDisciplineController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri url = new Uri("http://45.136.6.177:1345/api");
        public SubDisciplineController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = url;
        }
        [HttpGet]
        public IActionResult AdminSubDiscipline(ListModelView list)
        {
            List<SubDisciplineViewModel> subdisciplineViewModels = new List<SubDisciplineViewModel>();
            List<DisciplineViewModel> disciplineViewModels = new List<DisciplineViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/SubDisciplines/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                subdisciplineViewModels = JsonConvert.DeserializeObject<List<SubDisciplineViewModel>>(data);
                list.SubDisciplines = subdisciplineViewModels;

            }
            HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Discipline/Get").Result;
            if (response2.IsSuccessStatusCode)
            {
                var data = response2.Content.ReadAsStringAsync().Result;
                disciplineViewModels=JsonConvert.DeserializeObject<List<DisciplineViewModel>>(data);
                list.Disciplines = disciplineViewModels;
            }

            return View(list);
        }
        [HttpPost]
        public IActionResult AdminSubDiscipline(SubDisciplineViewModel subdiscipline)
        {
            var json = JsonConvert.SerializeObject(subdiscipline);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/SubDisciplines/Post", content).Result;
            if (response.IsSuccessStatusCode)
            {
               
                return RedirectToAction("AdminSubDiscipline");
            }
            else
            {
                TempData["message"] = "Bir Hata Oluştu";
                return RedirectToAction("AdminSubDiscipline");

            }
        }
        [HttpGet("UpdateSubDiscipline")]
        public IActionResult UpdateSubDiscipline()
        {
            return RedirectToAction("AdminSubDiscipline");
        }
        [HttpPost("UpdateSubDiscipline")]
        public IActionResult UpdateSubDiscipline(SubDisciplineViewModel subdiscipline)
        {
            var json = JsonConvert.SerializeObject(subdiscipline);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/SubDisciplines/Update", content).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Başarıyla güncellendi";
                return RedirectToAction("AdminSubDiscipline", "SubDiscipline");
            }
            else
            {
                TempData["Message"] = "Bir hata oluştu";
                return RedirectToAction("AdminSubDiscipline", "SubDiscipline");
            }
        }
        [HttpGet]
        public IActionResult AddSubDiscipline()
        {
            ListModelView list = new ListModelView();
            List<DisciplineViewModel> disciplines=new List<DisciplineViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Discipline/Get").Result;
            if(response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                disciplines = JsonConvert.DeserializeObject<List<DisciplineViewModel>>(data);
                list.Disciplines = disciplines;
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult AddSubDiscipline(SubDisciplineViewModel subdiscipline) {
            var json = JsonConvert.SerializeObject(subdiscipline);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/SubDisciplines/Post", content).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if(response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Başarıyla eklendi";
                return RedirectToAction("AdminPanel", "User");
            }
            else
            {
                TempData["Message"] = "Bir hata oluştu";
                return RedirectToAction("AdminPanel", "User");
            }
        
        
        }
    }
}
