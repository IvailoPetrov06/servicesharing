using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using System.Linq;

namespace servicesharing.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult BrakeSystem()
        {
            var services = _context.Services
                .Where(s => s.Name == "Смяна на спирачни дискове"
                         || s.Name == "Смяна на накладки"
                         || s.Name == "Диагностика на ABS")
                .ToList();

            return View(services);
        }

        public IActionResult EngineTransmission()
        {
            var services = _context.Services
                .Where(s => s.Name == "Смяна на масло"
                         || s.Name == "Смяна на горивен филтър"
                         || s.Name == "Проверка на инжекционната система")
                .ToList();

            return View(services);
        }

        public IActionResult Diagnostics()
        {
            var services = _context.Services
                .Where(s => s.Name == "Компютърна диагностика"
                         || s.Name == "Проверка на акумулатора"
                         || s.Name == "Проверка на спирачките")
                .ToList();

            return View(services);
        }

        public IActionResult GeneralRepairs()
        {
            var services = _context.Services
                .Where(s => s.Name == "Смяна на масло и маслен филтър"
                         || s.Name == "Смяна на въздушен филтър"
                         || s.Name == "Проверка и смяна на антифриз")
                .ToList();

            return View(services);
        }

        public IActionResult TiresAndWheels()
        {
            var services = _context.Services
                .Where(s => s.Name == "Смяна на гуми"
                         || s.Name == "Баланс на гуми"
                         || s.Name == "Ремонт на спукана гума")
                .ToList();

            return View(services);
        }

        public IActionResult Suspension()
        {
            var services = _context.Services
                .Where(s => s.Name == "Смяна на амортисьори"
                         || s.Name == "Смяна на пружини"
                         || s.Name == "Смяна на кормилни накрайници")
                .ToList();

            return View(services);
        }
    }
}
