using Microsoft.AspNetCore.Mvc;
using servicesharing.Models;
using servicesharing.ViewModels;

namespace servicesharing.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult List()
        {
            // Списък с услуги
            return View(new List<ServiceViewModel>());
        }

        public IActionResult Details(int id)
        {
            // Получаване на информация за услуга по ID
            return View(new ServiceViewModel());
        }
    }
}
