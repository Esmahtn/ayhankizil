using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data; // DbContext
using ayhankizil.Models; // Paylasim model

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
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(string AdSoyad, string Email, string PaylasimMetni)
        {
            if (string.IsNullOrWhiteSpace(AdSoyad) || string.IsNullOrWhiteSpace(PaylasimMetni))
            {
                // Eksik bilgi varsa hata ver (basit şekilde BadRequest)
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

            // Başarılı kayıt sonrası Home/Index sayfasına yönlendir
            return RedirectToAction("Index", "Home");
        }
    }
}
