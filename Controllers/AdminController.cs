using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using servicesharing.Data.Entities;

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
}
