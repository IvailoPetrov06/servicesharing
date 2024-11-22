using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using servicesharing.ViewModels;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data.Entities;

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
            // Load services into ViewData for the dropdown
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
                TempData["SuccessMessage"] = "Reservation successfully created!";
                return RedirectToAction("Index");
            }

            // Reload services if there's an error
            ViewData["Services"] = _context.Services.ToList();
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
                ReservationDate = reservation.ReservationDate,
                Services = _context.Services.ToList()
            };

            // Зареждаме всички услуги от базата данни и ги подаваме към ViewData
            ViewData["Services"] = _context.Services.ToList();
            return View(model);
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

                // Уверяваме се, че е избрана валидна услуга
                if (model.ServiceId == null || model.ServiceId == 0)
                {
                    ModelState.AddModelError("ServiceId", "Моля, изберете валидна услуга.");
                    ViewData["Services"] = _context.Services.ToList();
                    return View(model);
                }

                // Обновяваме резервацията с новите стойности
                reservation.ServiceId = model.ServiceId;
                reservation.CustomerEmail = model.CustomerEmail;
                reservation.ReservationDate = model.ReservationDate;

                // Записваме промените в базата данни
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Резервацията е успешно редактирана!";
                return RedirectToAction("Index");
            }

            // Ако има грешка, презареждаме услугите и връщаме потребителя към формата с текущите данни
            ViewData["Services"] = _context.Services.ToList();
            return View(model);
        }

        // GET: Reservation/Delete/5
        public IActionResult Delete(int id)
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

            return View(model);
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

            TempData["SuccessMessage"] = "Reservation successfully deleted!";
            return RedirectToAction("Index");
        }
    }
}
