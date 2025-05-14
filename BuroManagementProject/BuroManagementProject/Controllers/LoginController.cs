using BuroManagementProject.Data;
using BuroManagementProject.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BuroManagementProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly KisilerData _data;

        public LoginController(IConfiguration config)
        {
            _data = new KisilerData(config);
        }

        public IActionResult Aut()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Aut(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Hata = "Lütfen tüm alanları doldurunuz.";
                return View();
            }


            var kisilerList = _data.GetKisiler();

            var kullanıcı = kisilerList.FirstOrDefault(k =>
                k.Eposta == email && k.Sifre == password);

            if (kullanıcı != null)
            {
                int? controlValue = _data.GetRol_ID_By_Mail_Password(email, password);
                if (controlValue == null)
                {
                    ViewBag.Hata = "Geçersiz e-posta veya şifre.";
                    return View();
                }
                else if (controlValue == 1)
                {
                    return RedirectToAction("AvukatAut", "AvukatLogin");
                }
                else if (controlValue == 2)
                {
                    return RedirectToAction("MuvekkilAut", "MuvekkilLogin");
                }

            }
            else
            {
                ViewBag.Hata = "Geçersiz e-posta veya şifre.";

               return View();
            }

            return View();
        }

    }
}
