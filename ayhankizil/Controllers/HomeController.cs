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
            const int pageSize = 10; // Sayfa baþýna kaç paylaþým gösterilecek

            try
            {
                // SADECE ONAYLI PAYLAÞIMLARI GETÝR (Onayli = true)
                var query = _context.Paylasimlar
                                   .Where(p => p.Onayli == true) // Sadece onaylý olanlar
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

                // AJAX isteði mi kontrol et
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // Sadece partial view döndür (AJAX için)
                    return View(viewModel);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Hata durumunda boþ model döndür
                var emptyModel = new PaylasimViewModel
                {
                    Paylasimlar = new List<Paylasim>(),
                    CurrentPage = 1,
                    TotalPages = 0
                };

                TempData["Error"] = "Paylaþýmlar yüklenirken bir hata oluþtu.";
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