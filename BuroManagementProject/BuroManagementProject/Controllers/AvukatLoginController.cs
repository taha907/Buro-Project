using BuroManagementProject.Data;
using BuroManagementProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuroManagementProject.Controllers
{
    public class AvukatLoginController : Controller
    {
        private readonly KisilerData _data;
        public AvukatLoginController(IConfiguration config)
        {
            _data = new KisilerData(config);
        }
        public IActionResult AvukatAut()
        {
            return View();
        }
        
        
         [HttpPost]
        public IActionResult AvukatAut(Kisiler k,AdresAvukat a)
        {
            var sonuc = _data.AvukatKayit(k, a); // string değer dönüyor
            if(sonuc != "Başarılı")
            {
                ViewBag.KayitHatasi = sonuc;
                return View(k); // Kullanıcıyı aynı form ekranına döndür
            }
            return RedirectToAction("Aut", "Login");
        }
         

    }

}