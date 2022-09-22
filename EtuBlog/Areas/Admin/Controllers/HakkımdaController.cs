using EtuBlog.Areas.Admin.Filters;
using EtuBlog.Data;
using EtuBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HakkımdaController : Controller
    {
        private readonly EtuBlogContext db;

        public HakkımdaController(EtuBlogContext context)
        {
            db = context;
        }

        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult HakkımdaDuzenle()
        {
            var hakkımda = db.Hakkımda.FirstOrDefault();
            return View(hakkımda);
        }

        [HttpPost]
        [Route("Admin/Hakkımda/HakkımdaDuzenle")]
        [ServiceFilter(typeof(AdminUserSecurityAttribute))]
        public IActionResult HakkımdaDuzenle(Hakkımda bilgiler)
        {
            var duzenle = db.Hakkımda.Where(a => a.id == bilgiler.id).FirstOrDefault();

            duzenle.hakkımdaIcerik = bilgiler.hakkımdaIcerik;

            db.SaveChanges();

            return RedirectToAction("HakkımdaDuzenle", "Hakkımda");
        }

    }
}
