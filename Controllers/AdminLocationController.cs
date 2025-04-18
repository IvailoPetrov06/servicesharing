using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System.Threading.Tasks;

namespace servicesharing.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminLocationController : Controller
    {
        private readonly AppDbContext _context;

        public AdminLocationController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _context.Locations.ToListAsync();
            return View("~/Views/Admin/ManageLocations.cshtml", locations);
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/CreateLocation.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/CreateLocation.cshtml", location);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();

            return View("~/Views/Admin/EditLocation.cshtml", location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/EditLocation.cshtml", location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
