using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using servicesharing.Data;
using servicesharing.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace servicesharing.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("ManageUsers");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("ManageUsers");
            }

            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email.ToUpper();
            user.FullName = model.FullName;

            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["Message"] = "Имейлът беше успешно обновен!";
            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("ManageUsers");
            }

            // 🔥 Премахни всички стари Identity роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (newRole != "Клиент")
            {
                // ✅ Само ако НЕ е клиент, тогава добави роля в Identity
                await _userManager.AddToRoleAsync(user, newRole);
            }

            // 📝 Обнови колоната "Role" в базата (визуална роля)
            user.Role = newRole;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = $"Ролята на {user.FullName} беше променена на {newRole}.";
            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public IActionResult DeleteUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("ManageUsers");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Message"] = "Потребителският акаунт е успешно изтрит.";
            return RedirectToAction("ManageUsers");
        }
    }
}
