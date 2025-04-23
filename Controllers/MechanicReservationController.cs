using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "Автомеханик")]
public class MechanicReservationController : Controller
{
    private readonly AppDbContext _context;

    public MechanicReservationController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Manage()
    {
        var reservations = _context.Reservations.Include(r => r.User).ToList();
        return View("~/Views/Mechanic/ManageReservations.cshtml", reservations);
    }

    [HttpPost]
    public IActionResult ChangeReservationStatus(int reservationId, string newStatus)
    {
        var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
        {
            TempData["Error"] = "Резервацията не е намерена.";
            return RedirectToAction("Manage");
        }

        reservation.Status = Enum.Parse<ReservationStatus>(newStatus);
        _context.SaveChanges();

        TempData["Message"] = "Статусът беше обновен успешно.";
        return RedirectToAction("Manage");
    }

    [HttpGet]
    public IActionResult Remove(int id)
    {
        var reservation = _context.Reservations
            .Include(r => r.User)
            .FirstOrDefault(r => r.Id == id);

        if (reservation == null)
        {
            TempData["Error"] = "Резервацията не е намерена.";
            return RedirectToAction("Manage");
        }

        return View("~/Views/Mechanic/RemoveReservation.cshtml", reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation == null)
        {
            TempData["Error"] = "Резервацията не е намерена.";
        }
        else
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            TempData["Message"] = "Резервацията беше успешно изтрита.";
        }

        return RedirectToAction("Manage");
    }
}
