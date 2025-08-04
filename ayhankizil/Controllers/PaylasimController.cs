using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data; // Veri tabanı context'in burada olmalı
using ayhankizil.Models; // Paylasim modelin burada olmalı

namespace ayhankizil.Controllers
{
    public class PaylasimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaylasimController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Ekle(string AdSoyad, string Email, string PaylasimMetni)
        {
            if (string.IsNullOrWhiteSpace(AdSoyad) || string.IsNullOrWhiteSpace(PaylasimMetni))
            {
                // Eksik bilgi varsa hata ver
                return BadRequest("Ad Soyad ve Paylaşım metni boş bırakılamaz.");
            }

            var yeniPaylasim = new Paylasim
            {
                AdSoyad = AdSoyad,
                Email = Email,
                Icerik = PaylasimMetni
            };

            _context.Paylasimlar.Add(yeniPaylasim);
            _context.SaveChanges();

            // Kayıt sonrası anasayfaya dön
            return RedirectToAction("Index", "Home");
        }
    }
}
