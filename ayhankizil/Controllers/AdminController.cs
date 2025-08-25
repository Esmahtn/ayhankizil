using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;
using ayhankizil.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ayhankizil.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Admin giriş sayfası
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa ana sayfaya yönlendir
            if (IsAdmin())
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Kullanıcı adı ve şifre gereklidir!";
                    return View();
                }

                // Veritabanından admin kullanıcıyı bul
                var admin = await _db.Admins
                    .FirstOrDefaultAsync(a => a.Username == username && a.IsActive);

                if (admin == null)
                {
                    ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                    return View();
                }

                // Şifreyi doğrula
                if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                {
                    ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                    return View();
                }

                // Başarılı giriş
                HttpContext.Session.SetString("AdminId", admin.Id.ToString());
                HttpContext.Session.SetString("AdminUsername", admin.Username);

                // Son giriş tarihini güncelle
                admin.LastLoginDate = DateTime.Now;
                await _db.SaveChangesAsync();

                TempData["Success"] = "Başarıyla giriş yaptınız!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Loglama yapabilirsin
                ViewBag.Error = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return View();
            }
        }

        // TÜM PAYLASIMALRI GÖSTER - HEM ONAYLI HEM ONAYSIZ
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            try
            {
                // TÜM paylaşımları getir (hem onaylı hem onaysız)
                var tumPaylasimlar = await _db.Paylasimlar
                                        .OrderByDescending(p => p.Id)
                                        .ToListAsync();

                return View(tumPaylasimlar);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Paylaşımlar yüklenirken bir hata oluştu.";
                return View(new List<Paylasim>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            try
            {
                var yorum = await _db.Paylasimlar.FindAsync(id);
                if (yorum != null)
                {
                    yorum.Onayli = true; // Onaylandı olarak işaretle
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Paylaşım başarıyla onaylandı!";
                }
                else
                {
                    TempData["Error"] = "Paylaşım bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Paylaşım onaylanırken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        // YENİ: ONAYDAN KALDIRMA FONKSİYONU
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnayKaldir(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            try
            {
                var yorum = await _db.Paylasimlar.FindAsync(id);
                if (yorum != null)
                {
                    yorum.Onayli = false; // Onaydan kaldır
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Paylaşım onaydan kaldırıldı!";
                }
                else
                {
                    TempData["Error"] = "Paylaşım bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "İşlem gerçekleştirilirken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            try
            {
                var yorum = await _db.Paylasimlar.FindAsync(id);
                if (yorum != null)
                {
                    // Fotoğrafları da sil
                    var fotolar = new[] { yorum.Foto1, yorum.Foto2, yorum.Foto3, yorum.Foto4 };
                    foreach (var foto in fotolar.Where(f => !string.IsNullOrEmpty(f)))
                    {
                        var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", foto.TrimStart('/'));
                        if (System.IO.File.Exists(dosyaYolu))
                        {
                            System.IO.File.Delete(dosyaYolu);
                        }
                    }

                    _db.Paylasimlar.Remove(yorum);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Paylaşım başarıyla silindi!";
                }
                else
                {
                    TempData["Error"] = "Paylaşım bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Paylaşım silinirken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        // Çıkış yap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }

        // Admin kontrolü
        private bool IsAdmin()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            return !string.IsNullOrEmpty(adminId);
        }

        // İlk admin kullanıcı oluşturma metodu (Sadece development için)
        public async Task<IActionResult> CreateFirstAdmin()
        {
            // Production'da bu metodu kaldır!
#if DEBUG
            try
            {
                // Zaten admin varsa oluşturma
                if (await _db.Admins.AnyAsync())
                {
                    return Json(new { success = false, message = "Admin zaten mevcut!" });
                }

                // Şifre ve username'i aynı değişkende tanımla
                string username = "admin";
                string password = "1234"; // İstediğin şifreyi buraya yaz

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);

                var admin = new Admin
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _db.Admins.Add(admin);
                await _db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = $"İlk admin oluşturuldu! Username: {username}, Password: {password}",
                    username = username,
                    password = password
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
#else
            return NotFound();
#endif
        }

        // Admin sıfırlama metodu (Development için)
        public async Task<IActionResult> ResetAdmin()
        {
#if DEBUG
            try
            {
                // Tüm adminleri sil
                var admins = await _db.Admins.ToListAsync();
                _db.Admins.RemoveRange(admins);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Tüm adminler silindi! Şimdi CreateFirstAdmin'e gidebilirsin." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
#else
            return NotFound();
#endif
        }

        // Veritabanı test metodu
        public async Task<IActionResult> TestDb()
        {
#if DEBUG
            try
            {
                var adminCount = await _db.Admins.CountAsync();
                var admins = await _db.Admins.Select(a => new { a.Id, a.Username, a.CreatedDate, a.IsActive }).ToListAsync();

                return Json(new
                {
                    success = true,
                    message = $"Admins tablosu var! Kayıt sayısı: {adminCount}",
                    admins = admins
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
#else
            return NotFound();
#endif
        }

        // Şifre değiştirme sayfası
        public IActionResult ChangePassword()
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            try
            {
                if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                {
                    ViewBag.Error = "Tüm alanlar zorunludur!";
                    return View();
                }

                if (newPassword != confirmPassword)
                {
                    ViewBag.Error = "Yeni şifreler eşleşmiyor!";
                    return View();
                }

                if (newPassword.Length < 6)
                {
                    ViewBag.Error = "Yeni şifre en az 6 karakter olmalı!";
                    return View();
                }

                var adminId = int.Parse(HttpContext.Session.GetString("AdminId"));
                var admin = await _db.Admins.FindAsync(adminId);

                if (admin == null || !BCrypt.Net.BCrypt.Verify(currentPassword, admin.PasswordHash))
                {
                    ViewBag.Error = "Mevcut şifre yanlış!";
                    return View();
                }

                // Yeni şifreyi hash'le ve kaydet
                admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, 12);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Şifre başarıyla değiştirildi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şifre değiştirilirken bir hata oluştu.";
                return View();
            }
        }
    }
}