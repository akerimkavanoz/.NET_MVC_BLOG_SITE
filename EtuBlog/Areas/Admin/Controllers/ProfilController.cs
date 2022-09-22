using EtuBlog.Areas.Admin.Filters;
using EtuBlog.Data;
using EtuBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfilController : Controller
    {
        private readonly EtuBlogContext db;

        public ProfilController(EtuBlogContext context)
        {
            db = context;
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult ProfilDuzenle()
        {
            var yntduzenle = db.Yonetici.FirstOrDefault();
            return View(yntduzenle);
        }

        [HttpPost]
        [Route("Admin/Profil/ProfilDuzenle")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult ProfilDuzenle(Yonetici bilgiler, IFormFile ResimYolu)
        {
            var duzenle = db.Yonetici.Where(a => a.yoneticiID == bilgiler.yoneticiID).FirstOrDefault();

            if(String.IsNullOrEmpty(bilgiler.yoneticiAdi) || String.IsNullOrEmpty(bilgiler.yoneticiSoyadi) || String.IsNullOrEmpty(bilgiler.yoneticiKAdi) || String.IsNullOrEmpty(bilgiler.yoneticiSifre))
            {
                return Json("Butun alanlari doldurmadan duzenleme yapamazsiniz.");
            }

            else
            {
                duzenle.yoneticiAdi = bilgiler.yoneticiAdi;
                duzenle.yoneticiSoyadi = bilgiler.yoneticiSoyadi;
                duzenle.yoneticiKAdi = bilgiler.yoneticiKAdi;
                duzenle.yoneticiSifre = bilgiler.yoneticiSifre;

                if (ResimYolu != null)
                {
                    string imageExtension = Path.GetExtension(ResimYolu.FileName);
                    string imageName = Guid.NewGuid() + imageExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Image/YoneticiResimler/{imageName}");
                    using var stream = new FileStream(path, FileMode.Create);
                    ResimYolu.CopyToAsync(stream);
                    bilgiler.yoneticiResim = "/Image/YoneticiResimler/" + imageName;
                }

                db.SaveChanges();
            }

            return RedirectToAction("ProfilDuzenle", "Profil");
        }
    }
}
