using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;
using ayhankizil.Models;
using Microsoft.EntityFrameworkCore;

namespace ayhankizil.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 4; // 🔹 Sayfa başına 4 paylaşım gösterilecek

            try
            {
                // SADECE ONAYLI PAYLAŞIMLARI GETİR (Onayli = true)
                var query = _context.Paylasimlar
                                   .Where(p => p.Onayli == true) // Sadece onaylı olanlar
                                   .OrderByDescending(p => p.Id);

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var paylasimlar = await query
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

                var viewModel = new PaylasimViewModel
                {
                    Paylasimlar = paylasimlar,
                    CurrentPage = page,
                    TotalPages = totalPages
                };

                // 🔹 AJAX isteği mi kontrol et
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // 🔹 Sadece partial view döndür (AJAX için)
                    return PartialView("_PaylasilanlarPartial", viewModel);
                }

                // Normal sayfa yükleme
                return View(viewModel);
            }
            catch (Exception)
            {
                // Hata durumunda boş model döndür
                var emptyModel = new PaylasimViewModel
                {
                    Paylasimlar = new List<Paylasim>(),
                    CurrentPage = 1,
                    TotalPages = 0
                };

                TempData["Error"] = "Paylaşımlar yüklenirken bir hata oluştu.";
                return View(emptyModel);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
