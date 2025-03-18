using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

[Authorize] // ✅ Само за логнати потребители
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

    [HttpGet("MyReservations")] // ✅ Задава правилен път: /Profile/MyReservations
    public async Task<IActionResult> MyReservations()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "Потребителят не е намерен.";
            return RedirectToAction("Login", "Account");
        }

        var reservations = _context.Reservations
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.ReservationDate)
            .ToList();

        return View(reservations);
    }
}