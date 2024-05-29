using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace DekoBim.Controllers
{
    public class CompanysController : Controller
    {
        private readonly HttpClient _client;
        Uri url = new Uri("http://45.136.6.177:1345/api");
        public CompanysController()
        {
            _client = new HttpClient();
            _client.BaseAddress = url;
            
        }
   
        [HttpGet]
        public IActionResult AddAdminCompany()
        {
            ListModelView list=new ListModelView();
            List<CompanyViewModel> company = new List<CompanyViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Companys/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data=response.Content.ReadAsStringAsync().Result;
                company=JsonConvert.DeserializeObject<List<CompanyViewModel>>(data);
                list.Companys = company;
            }
            return View(list);
        }
        [HttpPost]  
        public IActionResult AddAdminCompany([FromForm] IFormFile? Photo, CompanyViewModel company) {
            if (Photo != null && Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    Photo.CopyTo(ms);
                    photoBytes = ms.ToArray();
                    company.Base64 = Convert.ToBase64String(photoBytes);
                }
            }
            var json = JsonConvert.SerializeObject(company);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Companys/Post", content).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AddAdminCompany", "Companys");
            }

            return RedirectToAction("AddAdminCompany", "Companys");
        }

        [HttpGet("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany()
        {
            return RedirectToAction("AddAdminCompany", "Companys");
        }
        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromForm] IFormFile? file,CompanyViewModel company)
        {
            if(file!=null && file.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    file.CopyTo(ms);
                    photoBytes = ms.ToArray();
                    company.Base64 = Convert.ToBase64String(photoBytes);
                }
            }
            var json = JsonConvert.SerializeObject(company);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Companys/Update", content).Result;
            var data=response.Content.ReadAsStringAsync().Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("AddAdminCompany", "Companys");
            }
            return RedirectToAction("AddAdminCompany", "Companys");

        }

        [HttpGet]
        public IActionResult MarkayaGoreUrun(int id)
        {
            CompanyViewModel company = new CompanyViewModel();
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress + "/Companys/Get/" + id).Result;
            var data = response2.Content.ReadAsStringAsync().Result;
            if (response2.IsSuccessStatusCode)
            {
                company = JsonConvert.DeserializeObject<CompanyViewModel>(data);
                HttpContext.Session.SetString("marka", company.Name_ ?? string.Empty);
                HttpContext.Session.SetString("base64", company.Base64 ?? string.Empty);
                HttpContext.Session.SetString("aciklama", company.CompanyDescription ?? string.Empty);
                HttpContext.Session.SetString("adres", company.CompanyAddress ?? string.Empty);
                HttpContext.Session.SetString("email", company.CompanyEmail ?? string.Empty);
                HttpContext.Session.SetString("tel", company.CompanyNumber ?? string.Empty);
                HttpContext.Session.SetString("website", company.CompanyUrl ?? string.Empty);
                HttpContext.Session.SetString("linkedin", company.LinkedinUrl ?? string.Empty);
                HttpContext.Session.SetString("facebook", company.FacebookUrl ?? string.Empty);
                HttpContext.Session.SetString("insta", company.InstragramUrl ?? string.Empty);
            }
        
            List<ProductsViewModel> products = new List<ProductsViewModel>();
            ListModelView list = new ListModelView();
            HttpResponseMessage response3 = _client.GetAsync(_client.BaseAddress + "/Companys/CompanyProductCount/" + id).Result;
            if (response3.IsSuccessStatusCode)
            {
                var count = int.Parse(response3.Content.ReadAsStringAsync().Result);
                HttpContext.Session.SetInt32("count", count);
            }
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Companys/MarkasiolanUrunler/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data2 = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductsViewModel>>(data2);
               
            }
            list.Products = products;
            return View(list);
        }

    }
}
