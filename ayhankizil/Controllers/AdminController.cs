using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;
using System.Linq;

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
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234") // Şimdilik sabit
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Yorum yönetimi
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") != "true")
                return RedirectToAction("Login");

            var yorumlar = _db.Paylasimlar
                             .OrderByDescending(p => p.Id)
                             .ToList();
            return View(yorumlar);
        }

        // Onayla
        public IActionResult Onayla(int id)
        {
            var yorum = _db.Paylasimlar.Find(id);
            if (yorum != null)
            {
                yorum.Onayli = true;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Sil
        public IActionResult Sil(int id)
        {
            var yorum = _db.Paylasimlar.Find(id);
            if (yorum != null)
            {
                _db.Paylasimlar.Remove(yorum);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
