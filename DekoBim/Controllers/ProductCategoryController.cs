using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DekoBim.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly INotyfService _notfy;
        Uri url = new Uri("http://45.136.6.177:1345/api");

        public ProductCategoryController(INotyfService notfy)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = url;
            _notfy = notfy;
        }
        [HttpGet]
        public IActionResult AddAdminProductCategory()
        {
            ListModelView list = new ListModelView();
            List<SubDisciplineViewModel> SubDisipline = new List<SubDisciplineViewModel>();
            List<CompanyViewModel> Companys = new List<CompanyViewModel>();
            List<ProductCategoryViewModel> categories=new List<ProductCategoryViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductCategory/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<ProductCategoryViewModel>>(data);
                list.Categories = categories;
            }
            HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/SubDisciplines/Get").Result;
            if (response2.IsSuccessStatusCode)
            {
                var data=response2.Content.ReadAsStringAsync().Result;
                SubDisipline=JsonConvert.DeserializeObject<List<SubDisciplineViewModel>>(data);
                list.SubDisciplines= SubDisipline;
            }
            return View(list);
        }
        [HttpPost("AddAdminProductCategory")]
        public IActionResult AddAdminProductCategory(ProductCategoryViewModel category)
        {
            var json = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ProductCategory/Post", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminPanel", "User");
            }
            return RedirectToAction("AdminPanel", "User");


        }

        [HttpGet("YeniOzellikEkle")]
        public IActionResult YeniOzellikEkle(int id)
        {
            ProductCategoryViewModel category = new ProductCategoryViewModel();
            HttpResponseMessage response = _httpClient.GetAsync(url + "/ProductCategory/Get/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category=JsonConvert.DeserializeObject<ProductCategoryViewModel>(data);
                HttpContext.Session.SetInt32("id", category.Id);
            }
            return View(category);
        }

        [HttpPost("YeniOzellikEkle")]
        public async Task<IActionResult> YeniOzellikEkle(ProductCategoryViewModel category)
        {
         category.Id=(int) HttpContext.Session.GetInt32("id");
            var json = JsonConvert.SerializeObject(category);

            StringContent content=new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            HttpResponseMessage response = _httpClient.PostAsync(url + "/ProductCategory/yeniOzellikEkle", content).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
          

                return RedirectToAction("AdminPanel", "User");
            }
            return RedirectToAction("AdminPanel", "User");

        }

        [HttpGet("UpdateProductCategory")]
        public IActionResult UpdateProductCategory()
        {
            return RedirectToAction("AddAdminProductCategory");
        }
        [HttpPost("UpdateProductCategory")]
        public IActionResult UpdateProductCategory(ProductCategoryViewModel category)
        {
            var json = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/ProductCategory/Update", content).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminPanel", "User");
            }
            else
            {
                return RedirectToAction("AdminPanel", "User");
            }
        }
    }
}
