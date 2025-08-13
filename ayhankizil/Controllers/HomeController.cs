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

        // Sayfal� listeleme
        public IActionResult Index(int page = 1)
        {
            int pageSize = 3; // sayfa ba��na payla��m say�s�

            // Sadece Onayli = true olanlar� say�yoruz
            int totalCount = _db.Paylasimlar.Count(p => p.Onayli);

            // Sadece Onayli = true olanlar� getiriyoruz, sayfalama ile
            var paylasimlar = _db.Paylasimlar
                                .Where(p => p.Onayli)
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
