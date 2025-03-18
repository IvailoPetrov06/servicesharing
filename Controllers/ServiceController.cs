using Microsoft.AspNetCore.Mvc;
using servicesharing.Data.Entities;
using System.Collections.Generic;

namespace servicesharing.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult BrakeSystem()
        {
            var services = new List<Service>
            {
                new Service { Name = "Смяна на спирачни дискове", Price = 80 },
                new Service { Name = "Смяна на накладки", Price = 50 },
                new Service { Name = "Диагностика на ABS", Price = 40 }
            };
            return View(services);

        }

        public IActionResult EngineTransmission()
        {
            var services = new List<Service>
            {
                new Service { Name = "Смяна на масло", Price = 120 },
                new Service { Name = "Смяна на горивен филтър", Price = 60 },
                new Service { Name = "Проверка на инжекционната система", Price = 80 }
            };
            return View(services);
        }

        public IActionResult Diagnostics()
        {
            var services = new List<Service>
            {
                new Service { Name = "Компютърна диагностика", Price = 50 },
                new Service { Name = "Проверка на акумулатора", Price = 20 },
                new Service { Name = "Проверка на спирачките", Price = 30 }
            };
            return View(services);
        }

        public IActionResult GeneralRepairs()
        {
            var services = new List<Service>
            {
                new Service { Name = "Смяна на масло и маслен филтър", Price = 70 },
                new Service { Name = "Смяна на въздушен филтър", Price = 30 },
                new Service { Name = "Проверка и смяна на антифриз", Price = 60 }
            };
            return View(services);
        }

        public IActionResult TiresAndWheels()
        {
            var services = new List<Service>
            {
                new Service { Name = "Смяна на гуми", Price = 30 },
                new Service { Name = "Баланс на гуми", Price = 25 },
                new Service { Name = "Ремонт на спукана гума", Price = 20 }
            };
            return View(services);
        }

        public IActionResult Suspension()
        {
            var services = new List<Service>
            {
                new Service { Name = "Смяна на амортисьори", Price = 100 },
                new Service { Name = "Смяна на пружини", Price = 80 },
                new Service { Name = "Смяна на кормилни накрайници", Price = 90 }
            };
            return View(services);
        }
    }
}