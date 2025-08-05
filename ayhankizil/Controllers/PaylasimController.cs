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
                return BadRequest("Ad Soyad ve Paylaşım metni boş bırakılamaz.");
            }

            var yeniPaylasim = new Paylasim
            {
                AdSoyad = AdSoyad,
                Email = Email,
                Icerik = PaylasimMetni
            };

            // Fotoğrafları işle (Foto1 - Foto4)
            for (int i = 1; i <= 4; i++)
            {
                var dosya = Request.Form.Files[$"Foto{i}"];
                if (dosya != null && dosya.Length > 0)
                {
                    var uzanti = Path.GetExtension(dosya.FileName);
                    var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
                    var yuklemeYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", dosyaAdi);

                    using (var stream = new FileStream(yuklemeYolu, FileMode.Create))
                    {
                        dosya.CopyTo(stream);
                    }

                    // Doğru model alanına ata
                    switch (i)
                    {
                        case 1: yeniPaylasim.Foto1 = "/uploads/" + dosyaAdi; break;
                        case 2: yeniPaylasim.Foto2 = "/uploads/" + dosyaAdi; break;
                        case 3: yeniPaylasim.Foto3 = "/uploads/" + dosyaAdi; break;
                        case 4: yeniPaylasim.Foto4 = "/uploads/" + dosyaAdi; break;
                    }
                }
            }

            _context.Paylasimlar.Add(yeniPaylasim);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
