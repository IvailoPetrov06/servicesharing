using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace servicesharing.Controllers
{
    [Authorize]
    public class PromotionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public PromotionController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 🌍 Всички могат да видят промоциите
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var promotions = await _context.Promotions
                .Include(p => p.Service)
                .Include(p => p.User)
                .Where(p => p.ValidUntil >= DateTime.Now)
                .ToListAsync();

            return View(promotions);
        }

        // 🛠️ Само вписани могат да създават
        public IActionResult Create()
        {
            ViewData["Services"] = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                promotion.UserId = user.Id;
                _context.Promotions.Add(promotion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Services"] = _context.Services.ToList();
            return View(promotion);
        }
    }
}
