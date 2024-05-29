using AspNetCoreHero.ToastNotification.Abstractions;
using DekoBim.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Paragraph = iTextSharp.text.Paragraph;
using Font = iTextSharp.text.Font;
using Document = iTextSharp.text.Document;
using Microsoft.AspNetCore.Hosting;
using Image = iTextSharp.text.Image;
using iTextSharp.text.pdf.draw;
using ClosedXML.Excel;
using System.Reflection.Metadata.Ecma335;
using DocumentFormat.OpenXml.InkML;
using System.Net;

namespace DekoBim.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly INotyfService _notfy;
        private readonly IWebHostEnvironment _webHostEnvironment;

        
        Uri url = new Uri("http://45.136.6.177:1345/api");

        public HomeController(INotyfService notfy, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = url;
            _notfy = notfy;
            _webHostEnvironment = webHostEnvironment;

        }


        public IActionResult Index()
        {
            HttpContext.Session.Remove("products");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Filter()
        {
            HttpContext.Session.Remove("count");
            int userEntryCount = HttpContext.Session.GetInt32("UserEntryCount") ?? 0;

            if (userEntryCount == 0)
            {
                HttpContext.Session.SetInt32("UserEntryCount", 1);
                var yanit = _httpClient.PostAsync(url + "/Istatistics/IncreaseViewCount", null).Result;

            }
           
            //bunu yaparsam üst üste api çaðýracak ve sayfa yavaþlayacak baþka birþey bulana kadar böyle yap
            ListModelView list = new ListModelView();
            List<ProductCategoryViewModel> categories = new List<ProductCategoryViewModel>();
            List<CompanyViewModel> companys = new List<CompanyViewModel>();
            List<DisciplineViewModel> disciplines = new List<DisciplineViewModel>();
            List<SubDisciplineViewModel> subDisciplines = new List<SubDisciplineViewModel>();
            List<FileTypesViewModel> fileTypes = new List<FileTypesViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductCategory/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<ProductCategoryViewModel>>(data);
            }

            HttpResponseMessage response2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Companys/Get").Result;
            if (response2.IsSuccessStatusCode)
            {
                var data = response2.Content.ReadAsStringAsync().Result;
                companys = JsonConvert.DeserializeObject<List<CompanyViewModel>>(data);
            }
            HttpResponseMessage response3 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Discipline/Get").Result;
            if (response3.IsSuccessStatusCode)
            {
                var data = response3.Content.ReadAsStringAsync().Result;
                disciplines = JsonConvert.DeserializeObject<List<DisciplineViewModel>>(data);
            }

            HttpResponseMessage response4 = _httpClient.GetAsync(_httpClient.BaseAddress + "/SubDisciplines/Get").Result;
            if (response4.IsSuccessStatusCode)
            {
                var data = response4.Content.ReadAsStringAsync().Result;
                subDisciplines = JsonConvert.DeserializeObject<List<SubDisciplineViewModel>>(data);
            }
            HttpResponseMessage response5 = _httpClient.GetAsync(_httpClient.BaseAddress + "/FileTypes/Get").Result;
            if (response5.IsSuccessStatusCode)
            {
                var data = response5.Content.ReadAsStringAsync().Result;
                fileTypes = JsonConvert.DeserializeObject<List<FileTypesViewModel>>(data);
            }
            list.Categories = categories;
            list.Companys = companys;
            list.Disciplines = disciplines;
            list.SubDisciplines = subDisciplines;
            list.FileTypes = fileTypes;
            return View(list);
        }

        [Route("Home/ClearSessionData")]
        public IActionResult ClearSessionData()
        {
            HttpContext.Session.Remove("products");
            // Diðer oturum verilerini temizleme iþlemleri...
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> Filter(ProductsViewModel product)
        {
         
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Product/Filter", content);
            var data = response.Content.ReadAsStringAsync().Result;
          
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<List<ProductsViewModel>>(responseContent);
                var productsJson = JsonConvert.SerializeObject(category);
                HttpContext.Session.SetString("products", productsJson);
                HttpContext.Session.SetInt32("sirketid", product.company.Id);
                HttpContext.Session.SetInt32("disiplinid", product.Category.subdicipline.discipline.Id);
                HttpContext.Session.SetInt32("altdisiplinid", product.Category.subdicipline.Id);
                HttpContext.Session.SetInt32("dosyaid", product.FileType.Id);
                HttpContext.Session.SetInt32("kategoriid", product.Category.Id);
                return RedirectToAction("Filter","Home");
            }
            else
            {
                HttpContext.Session.Remove("products");
                return RedirectToAction("Filter", "Home");

            }

        }
        [HttpPost]
public IActionResult SaveToSession([FromBody] SessionDataModel data)
{
    HttpContext.Session.SetString("SelectedBrands", JsonConvert.SerializeObject(data.SelectedBrands));
    HttpContext.Session.SetString("SelectedProducts", JsonConvert.SerializeObject(data.SelectedProducts));

    return Ok("Session data saved");
}
     
        [HttpGet]
        public async Task<IActionResult> IndirPdf(string SelectedProducts)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    string leftImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "dekogroup.png");
                    string rightImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "IAF.png");
                    var leftImage = Image.GetInstance(leftImagePath);
                    var rightImage = Image.GetInstance(rightImagePath);
                    leftImage.ScaleToFit(50f, 50f);
                    rightImage.ScaleToFit(50f, 50f);
                    leftImage.SetAbsolutePosition(40, 800);
                    rightImage.SetAbsolutePosition(500, 800);
                    document.Add(leftImage);
                    document.Add(rightImage);
                    var fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "arial.ttf");
                    var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var font = new Font(baseFont, 20, Font.BOLD);
                    var font2 = new Font(baseFont, 15);

                    var heading = new Paragraph("DEKO MÜHENDÝSLÝK LTD ÞTÝ.", font)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    };
                    document.Add(heading);
                    var lineSeparator = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1);
                    document.Add(new Chunk(lineSeparator));
                    var selectedproduct = SelectedProducts.Split(",");
                    foreach (var productId in selectedproduct)
                    {
                        int id = int.Parse(productId);
                        HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Product/markasartnamebilgigetir/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var product = JsonConvert.DeserializeObject<ProductsViewModel>(data);

                            document.Add(new Paragraph($"Ürün Ýsmi: {product.Name_}", font2));
                            document.Add(new Paragraph($"Ürün Kategorisi: {product.Category.Name_}", font2));
                            document.Add(new Paragraph($"Ürün Markasý: {product.company.Name_}", font2));

                            // Ürün bilgileri arasýna boþluk ekleme
                            document.Add(new Paragraph(" ", font2));
                        }

                    }
                    document.Add(new Chunk(lineSeparator));
                    document.Close();
                }
                var content = memoryStream.ToArray();
                return File(content, "application/pdf", "Þartname.pdf");
            }
        }
        [HttpGet]
        public async Task<IActionResult> IndirExcel(string selectedbrands)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("MARKALAR");
                worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.Orange;
                worksheet.Cell("B1").Style.Fill.BackgroundColor = XLColor.Orange;
                worksheet.Cell("B1").Value = "ÜRÜN ADI";
                worksheet.Cell("B1").Style.Font.Bold = true;
                
                worksheet.Cell("C1").Value = "MARKA ADI";
                worksheet.Cell("C1").Style.Font.Bold = true;
                worksheet.Cell("C1").Style.Fill.BackgroundColor = XLColor.AppleGreen;
                worksheet.Column("B").Width = 60; // Örnek olarak 20 birim geniþlik

                // B sütunu için geniþlik ayarlama
                worksheet.Column("C").Width = 50; // Örnek olarak 30 birim geniþlik
                // Baþlýk stillerini ayarlama
                var headerRange = worksheet.Range("B1:C1");
                headerRange.Style.Font.Bold = true;
            
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int currentRow = 3;

                // Virgülle ayrýlmýþ string'i diziye çevir
                var brandIds = selectedbrands.Split(',');

                foreach (var brandIdString in brandIds)
                {
                    // brandId'yi int'e çevir
                    int brandId = int.Parse(brandIdString);

                    // Her bir marka için veritabanýndan veya herhangi bir kaynaktan marka bilgilerini çek
                    HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Product/markasartnamebilgigetir/{brandId}");
                    if (response.IsSuccessStatusCode) //200 OK 404 Not Found 400 Bad Request  405 Method not allowed 
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var product = JsonConvert.DeserializeObject<ProductsViewModel>(data);
                        worksheet.Cell(currentRow, 2).Value = product.Name_; // Madde sütunu

                        worksheet.Cell(currentRow, 3).Value = product.company.Name_; // Önerilen Markalar sütunu
                        worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                   

                    // Hücre stillerini ayarlama
                    worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    var range = worksheet.Range($"B{currentRow}:C{currentRow}");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.OutsideBorderColor = XLColor.Black;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorderColor = XLColor.Black;

                    currentRow++;
                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MarkaListesi.xlsx");
                }
            }
        }






        [HttpPost]
        public IActionResult CreatePdfDocument(List<int> selectedProducts)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

               

                document.Open();
                string leftImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "dekogroup.png"); // Sol resmin adýný uygun þekilde deðiþtirin
                string rightImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "IAF.png"); // Sað resmin adýný uygun þekilde deðiþtirin

                Image leftImage = Image.GetInstance(leftImagePath);
                Image rightImage = Image.GetInstance(rightImagePath);

                // Resim boyutlarýný ve konumlarýný ayarla
                leftImage.ScaleAbsolute(50f, 50f);
                rightImage.ScaleAbsolute(50f, 50f);
                float availableWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                rightImage.SetAbsolutePosition(465, 760); // x koordinatý 525'ten 500'e düþürüldü
                float widthToUse = document.PageSize.Width * 0.15f; // Sayfanýn geniþliðinin %5'i
                leftImage.ScaleToFit(widthToUse, document.PageSize.Height); // Yükseklik, orantýyý korumak için geniþliðe göre ayarlanýr

                leftImage.SetAbsolutePosition(document.LeftMargin, document.PageSize.Height - document.TopMargin - leftImage.ScaledHeight);
                rightImage.ScaleToFit(document.PageSize.Width * 0.20f, document.PageSize.Height);
                document.Add(leftImage);
                document.Add(rightImage);
                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12);
                var heading = new Paragraph("DEKO MÜHENDÝSLÝK LTD ÞTÝ.", new Font(baseFont, 20, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 5
                };
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                LineSeparator line = new LineSeparator(1.0f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1);
                document.Add(heading);
                document.Add(new Chunk(line));

                var productsJson = HttpContext.Session.GetString("products");
                List<ProductsViewModel> products = new List<ProductsViewModel>();

                if (!string.IsNullOrEmpty(productsJson))
                {
                    products = JsonConvert.DeserializeObject<List<ProductsViewModel>>(productsJson);
                }
                foreach (var productId in selectedProducts)
                {
                    var data = products.FirstOrDefault(p => p.Id == productId);
                    if (data != null)
                    {
                        document.Add(new Paragraph("Ürün Adý: " + data.Name_, font) { Alignment = Element.ALIGN_CENTER });
                        document.Add(new Paragraph("Ürünün Kategorisi: " + data.Category.Name_, font) { Alignment = Element.ALIGN_CENTER });
                        document.Add(new Paragraph("Ürün Açýklamasý: " + data.description, font) { Alignment = Element.ALIGN_CENTER });

                        // Teknik özellikler
                        foreach (var ozellik in data.teknik_ozellikdata)
                        {
                            document.Add(new Paragraph(ozellik, font) { Alignment = Element.ALIGN_CENTER });
                        }
                        document.Add(new Paragraph("Ürünün Markasý: " + data.company.Name_, font) { Alignment = Element.ALIGN_CENTER });
                        document.Add(new Paragraph(" "));
                    }
                }

                document.Close();

                var content = memoryStream.ToArray();

                return File(content, "application/pdf", "Sartname.pdf");
            }
        }



        public IActionResult CreateWordDocument()
        {
            using (var document = DocX.Create("Þartname.docx"))
            {
                var heading = document.InsertParagraph("ÜRÜN ÞARTNAMESÝ")
                                           .FontSize(20) // Font boyutunu ayarlayýn
                                           .Bold() // Metni kalýn yapýn
                                           .Alignment = Alignment.center; // Baþlýðý ortala              
                var productsJson = HttpContext.Session.GetString("products");
                List<ProductsViewModel> products = new List<ProductsViewModel>();

                if (!string.IsNullOrEmpty(productsJson))
                {
                    products = JsonConvert.DeserializeObject<List<ProductsViewModel>>(productsJson);
                }
                foreach(var data in products)
                {
                    document.InsertParagraph("Ürün Adý: " + data.Name_).FontSize(20).Alignment = Alignment.center ;
                    document.InsertParagraph("Ürünün Kategorisi: " + data.Category.Name_).FontSize(20).Alignment=Alignment.center;
                    document.InsertParagraph("Ürün Açýklamasý: " + data.description).FontSize(20).Alignment = Alignment.center;
                    document.InsertParagraph("Ürünün Debi Deðeri: " + data.Debi).FontSize(20).Alignment = Alignment.center;
                    document.InsertParagraph("Ürünün Basýnç Kaybý: " + data.basinc_kaybi).FontSize(20).Alignment = Alignment.center;
                    document.InsertParagraph("Ürünün Markasý: " + data.company.Name_).FontSize(20).Alignment = Alignment.center;
                    document.InsertParagraph(" ");
                  

                }
                using (var stream = new MemoryStream())
                {
                    document.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Þartname.docx");
                }
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
