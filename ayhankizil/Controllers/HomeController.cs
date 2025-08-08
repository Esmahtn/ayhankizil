using System.Diagnostics;
using ayhankizil.Models;
using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;  // DbContext namespace
using System.Linq;

namespace ayhankizil.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;  // DbContext

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            // Veritabanýndan paylaþýmlarý çek, ID'ye göre azalan sýrayla (yeni önce)
            var paylasimlar = _db.Paylasimlar.OrderByDescending(p => p.Id).ToList();

            return View(paylasimlar);  // View'e model olarak gönder
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
