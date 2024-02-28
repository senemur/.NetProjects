using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UdemyKitapSitesi.Models;

//DATABASEDE EF TABLO OLUŞTURMAK İÇİN İLGİLİ MODEL SINIFLARI BURAYA EKLENMELİ

namespace UdemyKitapSitesi.Utility
{
    public class UygulamaDbContext : IdentityDbContext
    {
    public  UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

        public DbSet<KitapTuru> KitapTurleri { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Kiralama> Kiralamalar { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
}
