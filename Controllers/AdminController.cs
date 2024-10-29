using Microsoft.AspNetCore.Mvc;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Логика за административен панел
            return View();
        }

        public IActionResult ManageUsers()
        {
            // Логика за управление на потребители
            return View(new List<Users>());
        }
    }
}
