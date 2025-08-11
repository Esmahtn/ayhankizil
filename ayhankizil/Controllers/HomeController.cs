using Microsoft.AspNetCore.Mvc;
using ayhankizil.Data;
using ayhankizil.Models;
using System;
using System.Linq;

namespace ayhankizil.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Sayfalý listeleme
        public IActionResult Index(int page = 1)
        {
            int pageSize = 3; // sayfa baþýna paylaþým sayýsý

            int totalCount = _db.Paylasimlar.Count();

            var paylasimlar = _db.Paylasimlar
                                .OrderByDescending(p => p.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var model = new PaylasimViewModel
            {
                Paylasimlar = paylasimlar,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return View(model);
        }
    }
}
