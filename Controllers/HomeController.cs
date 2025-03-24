using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using servicesharing.Models;

namespace servicesharing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mechanic()
        {
            // Връщане на изглед за механика
            return View();
        }

        public IActionResult Service()
        {
            // Връщане на изглед за услугите
            return View();
        }

        public IActionResult Reservation()
        {
            // Връщане на изглед за резервация
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}
