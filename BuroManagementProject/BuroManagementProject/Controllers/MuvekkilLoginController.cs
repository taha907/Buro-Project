using BuroManagementProject.Data;
using BuroManagementProject.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace BuroManagementProject.Controllers
{
    public class MuvekkilLoginController : Controller
    {
        

        private readonly KisilerData _data;

        public MuvekkilLoginController(IConfiguration config)
        {
            _data = new KisilerData(config);
        }

        public IActionResult MuvekkilAut()
        {
            return View(new Kisiler());
        }

        [HttpPost]
        public IActionResult MuvekkilAut(Kisiler k)
        {
            var sonuc = _data.MuvekkilKayit(k); // string değer dönüyor

            if (sonuc != "Başarılı")
            {
                ViewBag.KayitHatasi = sonuc;
                return View(k); // Kullanıcıyı aynı form ekranına döndür
            }

            return RedirectToAction("Aut", "Login"); // Kayıt başarılıysa giriş ekranına yönlendir
        }

        public IActionResult Index()
        {
            ViewBag.ActiveIndex = "active";

            return View();
        }
        public IActionResult Profil()
        {
            ViewBag.ActiveProfil = "active";
            return View();
        }
        public IActionResult Avukatlarim()
        {
            ViewBag.ActiveAvukatlarim = "active";
            return View();
        }
        public IActionResult Davalarim()
        {
            ViewBag.ActiveDavalarim= "active";
            return View();
        }
        public IActionResult Durusmalarim()
        {
            ViewBag.ActiveDurusmalarim = "active";
            return View();
        }
        public IActionResult Hakkimizda()
        {
            ViewBag.ActiveHakkimizda = "active";
            return View();
        }
        public IActionResult Cikis()
        {
            
            return View();
        }


    }
}
