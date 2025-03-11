using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace servicesharing.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReservationController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📌 Показване на всички резервации
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations.Include(r => r.User).ToListAsync();
            return View(reservations);
        }

        // 📌 Форма за създаване на резервация
        public IActionResult Create()
        {
            ViewBag.Services = new[] { "Гуми и джанти", "Окачване и ходова част", "Двигател и трансмисия", "Спирачна система", "Диагностика и проверка", "Основни ремонти" };
            return View();
        }

        // 📌 Обработване на създаването на резервация
        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Пренасочване, ако няма логнат потребител
            }

            reservation.UserId = user.Id;
            reservation.Status = ReservationStatus.Pending; // По подразбиране е "Предстоящa"
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        // 📌 Форма за изтриване на резервация
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 📌 Показване на списък с резервации
        public async Task<IActionResult> List()
        {
            var reservations = await _context.Reservations.Include(r => r.User).ToListAsync();
            return View(reservations);
        }
    }
}
