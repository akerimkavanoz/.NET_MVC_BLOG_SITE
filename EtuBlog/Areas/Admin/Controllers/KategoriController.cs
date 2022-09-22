using EtuBlog.Areas.Admin.Filters;
using EtuBlog.Data;
using EtuBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KategoriController : Controller
    {
        private readonly EtuBlogContext db;

        public KategoriController(EtuBlogContext context)
        {
            db = context;
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult KategoriListele()
        {
            var listele = db.Kategori;
            return View(listele);
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/Kategori/KategoriEkle")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult KategoriEkle(Kategori bilgiler, string kategoriAdi)
        {
            if (String.IsNullOrEmpty(kategoriAdi) )
            {
                return Json("Kategori adi bos geicilemez.");
            }
            else
            {
                var ad = db.Kategori;
                foreach (var item in ad)
                {
                    if (item.kategoriAd.Equals(kategoriAdi))
                        return Json("Bu kategori zaten eklenmis.");

                    else
                        bilgiler.kategoriAd = kategoriAdi;
                }
                
            }

            db.Kategori.Add(bilgiler);
            db.SaveChanges();

            //return Json(new { Result = true, Message = "Kategori eklendi.", url = "Admin/Kategori/KategoriEkle" });
            return RedirectToAction("KategoriEkle", "Kategori");
        }


        [Route("Admin/Kategori/KategoriSil")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult KategoriSil(int id)
        {

            var silinecek = db.Kategori.Where(a => a.kategoriID == id).SingleOrDefault();
            db.Kategori.Remove(silinecek);
            db.SaveChanges();

            return RedirectToAction("KategoriEkle", "Kategori");   // buraya bakılacak
        }
    }
}
