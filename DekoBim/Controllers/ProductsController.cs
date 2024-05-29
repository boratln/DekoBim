using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace DekoBim.Controllers
{
    public class ProductsController : Controller
    {
        private readonly INotyfService _notfy;
        private readonly HttpClient _client;
        private readonly IWebHostEnvironment _environment;

        Uri url = new Uri("http://45.136.6.177:1345/api");
        public ProductsController(IWebHostEnvironment environment,INotyfService notfy)
        {
            _client = new HttpClient();
            _client.BaseAddress = url;
            _environment = environment;
            _notfy = notfy;
        }
        private string NormalizeFileName(string fileName)
        {
            fileName = fileName.Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s")
                               .Replace("ı", "i").Replace("ö", "o").Replace("ç", "c")
                               .Replace("Ğ", "G").Replace("Ü", "U").Replace("Ş", "S")
                               .Replace("İ", "I").Replace("Ö", "O").Replace("Ç", "C")
                               .Replace("'", "") 
                               .Replace("‘", "") 
                               .Replace("’", ""); 

            fileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));

            return fileName;
        }
        private async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "videos", subFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Dosya adını normalleştir
            var uniqueFileName = NormalizeFileName(file.FileName);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("videos", subFolder, uniqueFileName);
        }
        private async Task<string> SaveFileAsync2(IFormFile file, string subFolder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "Programs", subFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Dosya adını normalleştir
            var uniqueFileName = NormalizeFileName(file.FileName);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("Programs", subFolder, uniqueFileName);
        }
        [HttpGet]
        public IActionResult AdminProduct()
        {
            ListModelView list = new ListModelView();
            List<ProductsViewModel> products = new List<ProductsViewModel>();
            List<ProductCategoryViewModel> category = new List<ProductCategoryViewModel>();
            List<CompanyViewModel> company = new List<CompanyViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/ProductCategory/Get").Result;
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress + "/Companys/Get").Result;
            HttpResponseMessage response3 = _client.GetAsync(_client.BaseAddress + "/Product/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<List<ProductCategoryViewModel>>(data);
                list.Categories = category;
            }
            if (response2.IsSuccessStatusCode)
            {
                var data = response2.Content.ReadAsStringAsync().Result;
                company = JsonConvert.DeserializeObject<List<CompanyViewModel>>(data);
                list.Companys = company;
            }
            if (response3.IsSuccessStatusCode)
            {
                var data=response3.Content.ReadAsStringAsync().Result;
                products=JsonConvert.DeserializeObject<List<ProductsViewModel>>(data);
                list.Products = products;
            }
            return View(list);
        }
        [HttpPost]
        public async  Task<IActionResult> AdminProduct([FromForm] FileViewModel dosya, [FromForm] VideoFileUpload videos, ProductsViewModel product)
        {

            if (dosya.Photo != null && dosya.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    await dosya.Photo.CopyToAsync(ms);
                    photoBytes = ms.ToArray();
                    product.base64 = Convert.ToBase64String(photoBytes);
                }
            }
            if (dosya.revit != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] revitbytes;
                    await dosya.revit.CopyToAsync(ms);
                    revitbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(revitbytes);
                    product.revit = base64;
                    product.revittarih = DateTime.Now;
                    product.IsExistRevit= true;
                }
            }
           
          
            if (dosya.solidworks != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] solidbytes;
                    await dosya.solidworks.CopyToAsync(ms);
                    solidbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(solidbytes);

                    product.solidworks = base64;
                    product.solidtarih = DateTime.Now;
                    product.isExistSolidworks = true;
                }
            }
          
            if (dosya.autocad != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] autocadbytes;
                    await dosya.autocad.CopyToAsync(ms);
                    autocadbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(autocadbytes);

                    product.autocad = base64;
                    product.autocadtarih = DateTime.Now;
                    product.isExistAutocad = true;
                }
            }
        
            if (dosya.ifcformat != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] ifcbytes;
                    await dosya.ifcformat.CopyToAsync(ms);
                    ifcbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(ifcbytes);
                    product.ifcformat = base64;
                    product.ifctarih = DateTime.Now;
                    product.Isexistifcformat = true;
                }
            }
         
            if (dosya.sartnameler != null && dosya.sartnameler.Any())
            {
                product.sartnameler = new List<string>();
                var dosyalar = dosya.sartnameler.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sartnameler.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sartnameler.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.sertifikalar != null && dosya.sertifikalar.Any())
            {
                product.sertifakalar = new List<string>();
                var dosyalar = dosya.sertifikalar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sertifakalar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sertifakalar.Add(",");
                            }
                        }
                    }
                }
            }

            if (dosya.kullanimkilavuz != null && dosya.kullanimkilavuz.Any())
            {
                product.Kullanimkilavuzları = new List<string>();
                var dosyalar = dosya.kullanimkilavuz.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Kullanimkilavuzları.Add(base64);

                            if (i < dosyalar.Count - 1)
                            {
                                product.Kullanimkilavuzları.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.programlar != null && dosya.programlar.Any())
            {
                product.Programlar = new List<string>();
                var dosyalar = dosya.programlar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Programlar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.Programlar.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.DigerDokuman != null && dosya.DigerDokuman.Any())
            {
                product.DigerDokuman = new List<string>();
                foreach (var files in dosya.DigerDokuman)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync2(files, "OtherPrograms"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.DigerDokuman.Add(filePath);
                        if (files != dosya.DigerDokuman.Last())
                        {
                            product.DigerDokuman.Add(",");
                        }
                    }
                }
            }
            if (dosya.katalogfiyatliste != null && dosya.katalogfiyatliste.Any())
            {
                product.katalogfiyatliste = new List<string>();
                var dosyalar = dosya.katalogfiyatliste.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.katalogfiyatliste.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.katalogfiyatliste.Add(",");
                            }
                        }
                    }
                }
            }

            if (videos.TanitimVideos != null && videos.TanitimVideos.Any())
            {
                product.tanitimvideopath = new List<string>();
                foreach (var files in videos.TanitimVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "TanitimVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.tanitimvideopath.Add(filePath);
                        if (files != videos.TanitimVideos.Last())
                        {
                            product.tanitimvideopath.Add(",");
                        }
                    }
                }
            }
            if (videos.MontajVideos != null && videos.MontajVideos.Any())
            {
                product.montajvideopath = new List<string>();
                foreach (var files in videos.MontajVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "MontajVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.montajvideopath.Add(filePath);
                        if (files != videos.MontajVideos.Last())
                        {
                            product.montajvideopath.Add(",");
                        }
                    }
                }
            }
            var json = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "/Product/Post", content);
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminProduct", "Products");
            }
            return RedirectToAction("AdminProduct", "Products");
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            ListModelView list = new ListModelView();
            List<ProductCategoryViewModel> category = new List<ProductCategoryViewModel>();
            List<CompanyViewModel> company = new List<CompanyViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress+"/ProductCategory/Get").Result;
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress+"/Companys/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<List<ProductCategoryViewModel>>(data);
                list.Categories = category;
            }
            if (response2.IsSuccessStatusCode)
            {
                var data=response2.Content.ReadAsStringAsync().Result;
                company=JsonConvert.DeserializeObject<List<CompanyViewModel>>(data);
                list.Companys= company;
            }

           
            return View(list);
        }
        [HttpPost]
        public async  Task<IActionResult> AddProduct([FromForm] FileViewModel dosya, [FromForm]VideoFileUpload videos,ProductsViewModel product)
        {
            if (dosya.Photo != null && dosya.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    await dosya.Photo.CopyToAsync(ms);
                    photoBytes = ms.ToArray();
                    product.base64 = Convert.ToBase64String(photoBytes);
                }
            }
            if (dosya.revit != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] revitbytes;
                    await dosya.revit.CopyToAsync(ms);
                    revitbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(revitbytes);
                    product.revit = base64;
                    product.revittarih = DateTime.Now;
                }
            }
            if (dosya.solidworks != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] solidbytes;
                    await dosya.solidworks.CopyToAsync(ms);
                    solidbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(solidbytes);

                    product.solidworks = base64;
                    product.solidtarih = DateTime.Now;
                }
            }
            if (dosya.autocad != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] autocadbytes;
                    await dosya.autocad.CopyToAsync(ms);
                    autocadbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(autocadbytes);

                    product.autocad = base64;
                    product.autocadtarih = DateTime.Now;
                }
            }
            if (dosya.ifcformat != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] ifcbytes;
                    await dosya.ifcformat.CopyToAsync(ms);
                    ifcbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(ifcbytes);

                    product.ifcformat = base64;
                    product.ifctarih = DateTime.Now;
                }
            }
            if (dosya.sartnameler != null && dosya.sartnameler.Any())
            {
                product.sartnameler = new List<string>();
                var dosyalar = dosya.sartnameler.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sartnameler.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sartnameler.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.sertifikalar != null && dosya.sertifikalar.Any())
            {
                product.sertifakalar = new List<string>();
                var dosyalar = dosya.sertifikalar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sertifakalar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sertifakalar.Add(",");
                            }
                        }
                    }
                }
            }

            if (dosya.kullanimkilavuz != null && dosya.kullanimkilavuz.Any())
            {
                product.Kullanimkilavuzları = new List<string>();
                var dosyalar = dosya.kullanimkilavuz.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Kullanimkilavuzları.Add(base64);

                            if (i < dosyalar.Count - 1)
                            {
                                product.Kullanimkilavuzları.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.programlar != null && dosya.programlar.Any())
            {
                product.Programlar = new List<string>();
                var dosyalar = dosya.programlar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Programlar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.Programlar.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.DigerDokuman != null && dosya.DigerDokuman.Any())
            {
                product.DigerDokuman = new List<string>();
                foreach (var files in dosya.DigerDokuman)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync2(files, "OtherPrograms"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.DigerDokuman.Add(filePath);
                        if (files != dosya.DigerDokuman.Last())
                        {
                            product.DigerDokuman.Add(",");
                        }
                    }
                }
            }
            if (dosya.katalogfiyatliste != null && dosya.katalogfiyatliste.Any())
            {
                product.katalogfiyatliste = new List<string>();
                var dosyalar = dosya.katalogfiyatliste.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.katalogfiyatliste.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.katalogfiyatliste.Add(",");
                            }
                        }
                    }
                }
            }

            if (videos.TanitimVideos != null && videos.TanitimVideos.Any())
            {
                product.tanitimvideopath = new List<string>();
                foreach (var files in videos.TanitimVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "TanitimVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.tanitimvideopath.Add(filePath);
                        if (files != videos.TanitimVideos.Last())
                        {
                            product.tanitimvideopath.Add(",");
                        }
                    }
                }
            }
            if (videos.MontajVideos != null && videos.MontajVideos.Any())
            {
                product.montajvideopath = new List<string>();
                foreach (var files in videos.MontajVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "MontajVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.montajvideopath.Add(filePath);
                        if (files != videos.MontajVideos.Last())
                        {
                            product.montajvideopath.Add(",");
                        }
                    }
                }
            }
            var json = JsonConvert.SerializeObject(product);
            StringContent content=new StringContent(json,System.Text.Encoding.UTF8,"application/json");
            HttpResponseMessage response =  await _client.PostAsync(_client.BaseAddress+"/Product/Post", content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminProduct", "Products");
            }
            return RedirectToAction("AdminProduct", "Products");
        }

        [HttpGet("UpdateProduct")]
        public IActionResult UpdateProduct()
        {
            return RedirectToAction("AdminProduct", "Products");

        }
        [HttpPost("UpdateProduct")]
        public async  Task<IActionResult> UpdateProduct(ProductsViewModel product, [FromForm] FileViewModel dosya, [FromForm] VideoFileUpload videos)
        {
            if (dosya.Photo != null && dosya.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] photoBytes;
                    await dosya.Photo.CopyToAsync(ms);
                    photoBytes = ms.ToArray();
                    product.base64 = Convert.ToBase64String(photoBytes);
                }
            }
            if (dosya.revit != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] revitbytes;
                    await dosya.revit.CopyToAsync(ms);
                    revitbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(revitbytes);
                    product.revit = base64;
                    product.revittarih = DateTime.Now;
                }
            }
            if (dosya.solidworks != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] solidbytes;
                    await dosya.solidworks.CopyToAsync(ms);
                    solidbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(solidbytes);

                    product.solidworks = base64;
                    product.solidtarih = DateTime.Now;
                }
            }
            if (dosya.autocad != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] autocadbytes;
                    await dosya.autocad.CopyToAsync(ms);
                    autocadbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(autocadbytes);

                    product.autocad = base64;
                    product.autocadtarih = DateTime.Now;
                }
            }
            if (dosya.ifcformat != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] ifcbytes;
                    await dosya.ifcformat.CopyToAsync(ms);
                    ifcbytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(ifcbytes);

                    product.ifcformat = base64;
                    product.ifctarih = DateTime.Now;
                }
            }
            if (dosya.sartnameler != null && dosya.sartnameler.Any())
            {
                product.sartnameler = new List<string>();
                var dosyalar = dosya.sartnameler.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sartnameler.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sartnameler.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.sertifikalar != null && dosya.sertifikalar.Any())
            {
                product.sertifakalar = new List<string>();
                var dosyalar = dosya.sertifikalar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.sertifakalar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.sertifakalar.Add(",");
                            }
                        }
                    }
                }
            }

            if (dosya.kullanimkilavuz != null && dosya.kullanimkilavuz.Any())
            {
                product.Kullanimkilavuzları = new List<string>();
                var dosyalar = dosya.kullanimkilavuz.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Kullanimkilavuzları.Add(base64);

                            if (i < dosyalar.Count - 1)
                            {
                                product.Kullanimkilavuzları.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.programlar != null && dosya.programlar.Any())
            {
                product.Programlar = new List<string>();
                var dosyalar = dosya.programlar.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.Programlar.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.Programlar.Add(",");
                            }
                        }
                    }
                }
            }
            if (dosya.DigerDokuman != null && dosya.DigerDokuman.Any())
            {
                product.DigerDokuman = new List<string>();
                foreach (var files in dosya.DigerDokuman)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync2(files, "OtherPrograms"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.DigerDokuman.Add(filePath);
                        if (files != dosya.DigerDokuman.Last())
                        {
                            product.DigerDokuman.Add(",");
                        }
                    }
                }
            }
            if (dosya.katalogfiyatliste != null && dosya.katalogfiyatliste.Any())
            {
                product.katalogfiyatliste = new List<string>();
                var dosyalar = dosya.katalogfiyatliste.ToList();

                for (int i = 0; i < dosyalar.Count; i++)
                {
                    var dosyas = dosyalar[i];
                    if (dosyas.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await dosyas.CopyToAsync(ms);
                            var videoBytes = ms.ToArray();
                            var base64 = Convert.ToBase64String(videoBytes);
                            product.katalogfiyatliste.Add(base64);

                            // Son eleman hariç her elemanın ardından virgül ekle
                            if (i < dosyalar.Count - 1)
                            {
                                product.katalogfiyatliste.Add(",");
                            }
                        }
                    }
                }
            }

            if (videos.TanitimVideos != null && videos.TanitimVideos.Any())
            {
                product.tanitimvideopath = new List<string>();
                foreach (var files in videos.TanitimVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "TanitimVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.tanitimvideopath.Add(filePath);
                        if (files != videos.TanitimVideos.Last())
                        {
                            product.tanitimvideopath.Add(",");
                        }
                    }
                }
            }
            if (videos.MontajVideos != null && videos.MontajVideos.Any())
            {
                product.montajvideopath = new List<string>();
                foreach (var files in videos.MontajVideos)
                {
                    if (files.Length > 0)
                    {
                        // SaveFileAsync fonksiyonunu çağırarak dosyayı kaydet ve yolunu al
                        string filePath = await SaveFileAsync(files, "MontajVideos"); // SubFolder adını uygun şekilde değiştirebilirsiniz

                        // Dosya yollarını virgülle ayırarak listeye ekle
                        product.montajvideopath.Add(filePath);
                        if (files != videos.MontajVideos.Last())
                        {
                            product.montajvideopath.Add(",");
                        }
                    }
                }
            }
            var json = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "/Product/Update", content);
            var data = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminProduct", "Products");
            }
            return RedirectToAction("AdminProduct", "Products");
        }
        [HttpGet]
        public IActionResult ProductDetail(int id) {
            ProductsViewModel product = new ProductsViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress+"/Product/Get/" + id).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                 data = response.Content.ReadAsStringAsync().Result;
                product=JsonConvert.DeserializeObject<ProductsViewModel>(data);
            }
            HttpContext.Session.Remove("products");
            return View(product);
        }
    }
}
