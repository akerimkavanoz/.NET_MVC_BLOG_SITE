using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EtuBlog.Models
{
    public class Yonetici
    {
        [Key]
        public int yoneticiID { get; set; }
        public string yoneticiAdi { get; set; }
        public string yoneticiSoyadi { get; set; }
        public string yoneticiKAdi { get; set; }
        public string yoneticiSifre { get; set; }
        public string yoneticiResim { get; set; }
        public int rol { get; set; }
    }
}
