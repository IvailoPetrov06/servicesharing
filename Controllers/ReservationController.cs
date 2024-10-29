using Microsoft.AspNetCore.Mvc;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Create()
        {
            return View(new ReservationViewModel());
        }

        [HttpPost]
        public IActionResult Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Логика за създаване на резервация
                return RedirectToAction("List");
            }
            return View(model);
        }

        public IActionResult List()
        {
            return View(new List<ReservationViewModel>());
        }
    }
}
