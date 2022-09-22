using EtuBlog.Data;
using EtuBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtuBlog.Areas.Admin.Filters;

namespace EtuBlog.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class GirisController : Controller
    {
        private readonly EtuBlogContext db;

        public GirisController(EtuBlogContext context)
        {
            db = context;
        }

        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GirisSorgula([FromBody] Yonetici data)
        {
            try
            {
                string kAdi = data.yoneticiKAdi;
                string Parola = data.yoneticiSifre;
                System.Diagnostics.Debug.WriteLine(data.yoneticiKAdi);
                System.Diagnostics.Debug.WriteLine(data.yoneticiSifre);
                if (String.IsNullOrEmpty(Parola) && String.IsNullOrEmpty(kAdi))
                {
                   
                    return Json(new { Result = false, Message = "Kullanıcı Adınızı ve Şifrenizi Girmediniz!" });
                }
                else if (String.IsNullOrEmpty(kAdi))
                {
                   
                    return Json(new { Result = false, Message = "Kullanıcı Adınızı girmediniz!" });
                }
                else if (String.IsNullOrEmpty(Parola))
                {
                    
                    return Json(new { Result = false, Message = "Şifrenizi Girmediniz!" });
                }
                else
                {
                    
                    var kullanici = db.Yonetici.FirstOrDefault(x => x.yoneticiKAdi == kAdi && x.yoneticiSifre == Parola && x.rol == 1);

                    if (kullanici == null)
                    {
                       
                        return Json(new { Result = false, Message = "Kullanıcı Adınızı ya da Şifreyi hatalı girdiniz!" });
                    } 
                        


                    //Güvenlik açısından bilgileri şifreleyerek saklamamız daha doğru bir yöntemdir.
                    //Asp.Net Membership yapısı, bu güvenliği sunmaktadır.

                    HttpContext.Session.SetInt32("Kullanici_ID", kullanici.yoneticiID); // Yeni bir session oluşturma.
                    HttpContext.Session.SetString("Ad", kullanici.yoneticiAdi);
                    HttpContext.Session.SetString("Kullanıcı Adı", kullanici.yoneticiKAdi);
                    HttpContext.Session.SetString("Soyad", kullanici.yoneticiSoyadi);
                    HttpContext.Session.SetString("Resim", kullanici.yoneticiResim);
                    HttpContext.Session.SetInt32("Rol", kullanici.rol);

                    HttpContext.Session.SetInt32("YoneticiRol", kullanici.rol);

                    //Burada eğer, kullanıcı bilgileri, sistemde eşleşirse, geriye girişin başarılı
                    //olduğuna dair bir mesaj ve 3 saniye sonra, ana sayfaya yönlendirecek bir
                    //javascript kodu ekliyoruz.        
                    return Json(new { Result = true, Message = "Başarıyla Giriş Yaptınız. Yönlendiriliyorsunuz...", url = "Giris/Anasayfa" });
                    // return "Başarıyla Giriş Yaptınız. Yönlendiriliyorsunuz...<script type='text/javascript'>setTimeout(function(){window.location='/Admin/Giris/AnaSayfa'},2000);</script>";
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }

        public IActionResult OturumuKapat()
        {
            HttpContext.Session.Clear(); // Tüm sessionları temizle
            return View("Giris");
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult Anasayfa()
        {
            return View();
        }

        public IActionResult Hata()
        {
            return View();
            //return Redirect("/Admin/Giris");
        }

    }

}
