using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EtuBlog.Models;

namespace EtuBlog.Data
{
    public class EtuBlogContext : DbContext
    {
        public EtuBlogContext (DbContextOptions<EtuBlogContext> options)
            : base(options)
        {
        }

        public DbSet<EtuBlog.Models.Makale> Makale { get; set; }
        public DbSet<EtuBlog.Models.Yonetici> Yonetici { get; set; }
        public DbSet<EtuBlog.Models.Yazar> Yazar { get; set; }
        public DbSet<EtuBlog.Models.Kategori> Kategori { get; set; }
        public DbSet<EtuBlog.Models.Hakkımda> Hakkımda { get; set; }

    }
}
