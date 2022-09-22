using EtuBlog.Areas.Admin.Filters;
using EtuBlog.Data;
using EtuBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MakaleYonetimiController : Controller
    {
        private readonly EtuBlogContext db;

        public MakaleYonetimiController(EtuBlogContext context)
        {
            db = context;
        }

        [Route("Admin/MakaleYonetimi/MakaleListele")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult MakaleListele()
        {
            var listele = db.Makale;

            //if (listele != null)
            //{
            //    var yazar = db.Yazar.Where(a => a.yazarID == listele.yazarID).FirstOrDefault();
            //    listele.Yazar = yazar;
            //}

            return View(listele);
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult MakaleEkle()
        {
            //bir list oluştuyoruz selectlistitem tipi alacak
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            //foreach ile db.Categories deki kategorileri listemize ekliyoruz
            foreach (var item in db.Kategori.ToList())
            {   //Text = Görünen kısımdır. Kategori ismini yazdıyoruz
                //Value = Değer kısmıdır.ID değerini atıyoruz
                kategoriler.Add(new SelectListItem { Text = item.kategoriAd, Value = item.kategoriID.ToString() });
            }
            //Dinamik bir yapı oluşturup kategoriler list mizi view mize göndereceğiz
            //bunun için viewbag kullanıyorum
            ViewBag.Kategoriler = kategoriler;
            return View();
        }

        [HttpPost]
        [Route("Admin/MakaleYonetimi/MakaleEkle")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public async Task<IActionResult> MakaleEkle(Makale bilgiler, IFormFile ResimYolu, string makaleBaslik, string makaleIcerik, int kategoriler)
        {  
            if(ResimYolu == null || String.IsNullOrEmpty(makaleBaslik) || String.IsNullOrEmpty(makaleIcerik) || kategoriler == null)
            {
                return Json("Butun alanlari doldurmadan ekleme yapamazsiniz.");
            }

            else
            {
                bilgiler.yazarID = (int)HttpContext.Session.GetInt32("Kullanici_ID");
                bilgiler.makaleEklenmeTarihi = DateTime.Now;
                bilgiler.makaleGoruntulenmeSayisi = 0;
                bilgiler.makaleBaslik = makaleBaslik;
                bilgiler.makaleIcerik = makaleIcerik;
                bilgiler.kategoriID = kategoriler;

                if (ResimYolu != null)
                {
                    string imageExtension = Path.GetExtension(ResimYolu.FileName);
                    string imageName = Guid.NewGuid() + imageExtension;
                    System.Diagnostics.Debug.WriteLine(imageName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Image/MakaleResimleri/{imageName}");
                    using var stream = new FileStream(path, FileMode.Create);
                    await ResimYolu.CopyToAsync(stream);
                    bilgiler.makaleResim = "/Image/MakaleResimleri/" + imageName;
                }

                db.Makale.Add(bilgiler);
                db.SaveChanges();
                return RedirectToAction("MakaleListele", "MakaleYonetimi");
            }
          
        }

        [Route("Admin/MakaleYonetimi/MakaleSil")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult MakaleSil(int id)
        {
            var silinecek = db.Makale.Where(a => a.makaleID == id).SingleOrDefault();
            db.Makale.Remove(silinecek);
            db.SaveChanges();

            return RedirectToAction("MakaleListele", "MakaleYonetimi");   // buraya bakılacak
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult MakaleDuzenle(int id)
        {
            var duzenle = db.Makale.Where(a => a.makaleID == id).FirstOrDefault();
            return View(duzenle);

        }

        [HttpPost]
        [Route("Admin/MakaleYonetimi/MakaleDuzenle")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult MakaleDuzenle(Makale bilgiler, IFormFile ResimYolu)
        {
            var duzenle = db.Makale.Where(a => a.makaleID == bilgiler.makaleID).FirstOrDefault();

            if(String.IsNullOrEmpty(bilgiler.makaleBaslik) || String.IsNullOrEmpty(bilgiler.makaleIcerik))
            {
                return Json("Butun alanlari doldurmadan duzenleme yapamazsiniz.");
            }

            else
            {
                duzenle.makaleBaslik = bilgiler.makaleBaslik;
                duzenle.makaleEklenmeTarihi = DateTime.Now;
                duzenle.yazarID = (int)HttpContext.Session.GetInt32("Kullanici_ID");
                duzenle.makaleGoruntulenmeSayisi = bilgiler.makaleGoruntulenmeSayisi;
                duzenle.makaleIcerik = bilgiler.makaleIcerik;
                duzenle.kategoriID = bilgiler.kategoriID;


                if (ResimYolu != null)
                {
                    string imageExtension = Path.GetExtension(ResimYolu.FileName);
                    string imageName = Guid.NewGuid() + imageExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Image/MakaleResimleri/{imageName}");
                    using var stream = new FileStream(path, FileMode.Create);
                    ResimYolu.CopyToAsync(stream);
                    bilgiler.makaleResim = "/Image/MakaleResimleri/" + imageName;
                }
                db.SaveChanges();

            }

            return RedirectToAction("MakaleListele","MakaleYonetimi");
        }
    }
}