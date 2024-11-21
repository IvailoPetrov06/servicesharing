using Microsoft.AspNetCore.Mvc;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class MechanicController : Controller
    {
        public IActionResult Profile(int id)
        {
            var mechanic = new MechanicProfileViewModel
            {
                Id = 1,
                Name = "Иван Георгиев"
            };

            // Предаване на един механик в изгледа
            return View(new List<MechanicProfileViewModel> { mechanic });
        }

        public IActionResult List()
        {
            return View();
        }
    }
}
