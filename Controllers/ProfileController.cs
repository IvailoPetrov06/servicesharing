using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("Profile")]
public class ProfileController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public ProfileController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("MyReservations")]
    public async Task<IActionResult> MyReservations()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "Потребителят не е намерен.";
            return RedirectToAction("Login", "Account");
        }

        var reservations = await _context.Reservations
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();

        return View(reservations);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null || reservation.UserId != user.Id)
        {
            return Unauthorized();
        }

        return View(reservation);
    }

    [HttpPost("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null || reservation.UserId != user.Id)
        {
            return Unauthorized();
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Резервацията беше успешно изтрита.";
        return RedirectToAction("MyReservations");
    }
}
