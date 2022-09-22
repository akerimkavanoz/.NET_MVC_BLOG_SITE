using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Models
{
    public class Makale
    {
        [Key]
        public int makaleID { get; set; }
        public string makaleBaslik { get; set; }
        public string makaleIcerik { get; set; }
        public System.DateTime makaleEklenmeTarihi { get; set; }
        public int makaleGoruntulenmeSayisi { get; set; }
        public string makaleResim { get; set; }
        public int kategoriID { get; set; }
        public int yazarID { get; set; }
        public Yazar Yazar { get; set; }
        public Kategori Kategori { get; set; }
    }
}
