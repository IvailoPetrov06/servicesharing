using Microsoft.AspNetCore.Mvc;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class MechanicController : Controller
    {
        public IActionResult Profile(int id)
        {
            // Примерно извличане на данни за механика
            var mechanic = new MechanicProfileViewModel
            {
                Id = id,
                Name = "Примерен механик",
             
            };

            return View(mechanic);
        }

        public IActionResult List()
        {
            var mechanics = new List<MechanicProfileViewModel>
            {
                new MechanicProfileViewModel { Id = 1, Name = "Механик 1" },
                new MechanicProfileViewModel { Id = 2, Name = "Механик 2" },
            };

            return View(mechanics);
        }
    }
}
