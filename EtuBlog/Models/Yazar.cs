using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Models
{
    public class Yazar
    {
        [Key]
        public int yazarID { get; set; }
        public string yazarAdi { get; set; }
        public string yazarSoyadi { get; set; }
        public string yazarKAdi { get; set; }
        public string yazarSifre { get; set; }
        public string yazarResim { get; set; }
    }
}
