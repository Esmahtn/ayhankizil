using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;
using ayhankizil.Models;
using System.IO;
using System;

namespace ayhankizil.Controllers
{
    public class PaylasimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaylasimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Paylaşım ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(string AdSoyad, string Email, string PaylasimMetni)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AdSoyad) || string.IsNullOrWhiteSpace(PaylasimMetni))
                {
                    TempData["Error"] = "Ad Soyad ve Paylaşım metni boş bırakılamaz.";
                    return RedirectToAction("Index", "Home");
                }

                var yeniPaylasim = new Paylasim
                {
                    AdSoyad = AdSoyad.Trim(),
                    Email = Email?.Trim(),
                    Icerik = PaylasimMetni.Trim(),
                    Onayli = false, // Yeni eklenen paylaşımlar onaysız olarak eklensin
                    EklenmeTarihi = DateTime.Now // TARİH OTOMATİK EKLENİYOR

                };

                // Uploads klasörünün var olduğundan emin ol
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Fotoğrafları işle
                for (int i = 1; i <= 4; i++)
                {
                    var dosya = Request.Form.Files[$"Foto{i}"];
                    if (dosya != null && dosya.Length > 0)
                    {
                        // Dosya boyutu kontrolü (5MB = 5 * 1024 * 1024 bytes)
                        if (dosya.Length > 5 * 1024 * 1024)
                        {
                            TempData["Error"] = $"Foto{i} çok büyük. Maksimum 5MB olabilir.";
                            return RedirectToAction("Index", "Home");
                        }

                        // Dosya uzantısı kontrolü
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                        var uzanti = Path.GetExtension(dosya.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(uzanti))
                        {
                            TempData["Error"] = $"Foto{i} geçersiz format. Sadece resim dosyaları kabul edilir.";
                            return RedirectToAction("Index", "Home");
                        }

                        var dosyaAdi = $"{Guid.NewGuid()}{uzanti}";
                        var yuklemeYolu = Path.Combine(uploadsPath, dosyaAdi);

                        using (var stream = new FileStream(yuklemeYolu, FileMode.Create))
                        {
                            await dosya.CopyToAsync(stream);
                        }

                        // Dosya yolunu modele ata
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
                await _context.SaveChangesAsync();

                // Başarı mesajı ekle
                TempData["Success"] = "Paylaşımınız gönderildi. Onaylandıktan sonra yayınlanacaktır.";

                // Eklemeden sonra ana sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Paylaşım eklenirken bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}