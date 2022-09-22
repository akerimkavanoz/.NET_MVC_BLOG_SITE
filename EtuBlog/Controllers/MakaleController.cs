using EtuBlog.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Controllers
{
    public class MakaleController : Controller
    {
        private readonly EtuBlogContext db;

        public MakaleController(EtuBlogContext context)
        {
            db = context;
        }

        public IActionResult Makale()
        {
            return View();
        }
        
        public IActionResult MakaleDetay(int id)
        {
            var detay = db.Makale.First(a => a.makaleID == id);
            
            if (detay != null)
            {
                var yazar = db.Yazar.Where(a => a.yazarID == detay.yazarID).FirstOrDefault();
                detay.Yazar = yazar;
            }

            return View(detay);
        }

        [HttpPost]
        public JsonResult GetirMakale()
        {
            try
            {
                var makale = db.Makale.OrderByDescending(a => a.makaleEklenmeTarihi);
                
                return Json(new { data = makale, Result = true, Message = "Başarılı" });
            }
            catch (System.Exception ex)
            {

                return Json(new { Result = false, Message = ex.Message });
            }
        }
    }
}
