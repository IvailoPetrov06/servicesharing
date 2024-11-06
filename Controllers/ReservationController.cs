using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            var reservations = _context.Reservations.ToList();
            return View(reservations);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            // Зареждаме услугите в ViewData
            ViewData["Services"] = _context.Services.ToList();
            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reservation model)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Резервацията е успешно създадена!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Reservation/Edit/5
        public IActionResult Edit(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var model = new ReservationViewModel
            {
                ServiceId = reservation.ServiceId,
                CustomerEmail = reservation.CustomerEmail,
                ReservationDate = reservation.ReservationDate
            };

            // Зареждаме услугите в ViewData
            ViewData["Services"] = _context.Services.ToList();

            return View(model);  // Подаваме ReservationViewModel на изгледа
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = _context.Reservations.Find(id);
                if (reservation == null)
                {
                    return NotFound();
                }

                // Обновяваме стойностите на резервацията
                reservation.ServiceId = model.ServiceId;
                reservation.CustomerEmail = model.CustomerEmail;
                reservation.ReservationDate = model.ReservationDate;

                // Записваме промените в базата данни
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Резервацията е успешно редактирана!";
                return RedirectToAction("Index");
            }

            return View(model);  // Връщаме модела, ако има грешка
        }

        // GET: Reservation/Delete/5
        public IActionResult Delete(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Преобразуваме Reservation в ReservationViewModel
            var model = new ReservationViewModel
            {
                ServiceId = reservation.ServiceId,
                CustomerEmail = reservation.CustomerEmail,
                ReservationDate = reservation.ReservationDate
            };

            return View(model);  // Подаваме ReservationViewModel на изгледа
        }

        // POST: Reservation/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Резервацията е успешно изтрита!";
            return RedirectToAction("Index");
        }
    }
}
