using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using servicesharing.Data.Entities;
using servicesharing.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ManageUsers()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult EditUser(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            TempData["Error"] = "Потребителят не е намерен.";
            return RedirectToAction("ManageUsers");
        }

        return View(user);
    }

    [HttpPost]
    public IActionResult EditUser(User model)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
        if (user == null)
        {
            TempData["Error"] = "Потребителят не е намерен.";
            return RedirectToAction("ManageUsers");
        }

        user.Email = model.Email;
        user.FullName = model.FullName;

        _context.Users.Update(user);
        _context.SaveChanges();

        TempData["Message"] = "Информацията за потребителя е успешно обновена.";
        return RedirectToAction("ManageUsers");
    }

    [HttpGet]
    public IActionResult DeleteUser(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            TempData["Error"] = "Потребителят не е намерен.";
            return RedirectToAction("ManageUsers");
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        TempData["Message"] = "Потребителският акаунт е успешно изтрит.";
        return RedirectToAction("ManageUsers");
    }
}