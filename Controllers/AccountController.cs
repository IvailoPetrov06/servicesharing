using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using servicesharing.ViewModels;
using servicesharing.Data.Entities;
using System.Threading.Tasks;

namespace UsersApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Невалиден опит за влизане.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Redirect admin to Admin Dashboard
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Невалиден опит за влизане.");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign Admin role if the email matches the admin email
                    if (model.Email == "adminemail@gmail.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                TempData["Error"] = "Моля, въведете валиден имейл адрес.";
                return RedirectToAction("Profile");
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("Login");
            }

            var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await userManager.ChangeEmailAsync(user, newEmail, token);

            if (result.Succeeded)
            {
                user.UserName = newEmail; // Update the UserName if it's used as the login
                await userManager.UpdateAsync(user);

                TempData["Message"] = "Имейлът е успешно обновен.";
            }
            else
            {
                TempData["Error"] = "Грешка при обновяване на имейла.";
            }

            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        // POST: Delete Account
        [HttpPost]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Потребителят не е намерен.";
                return RedirectToAction("Login");
            }

            // Delete the user
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await signInManager.SignOutAsync();
                TempData["Message"] = "Акаунтът е успешно изтрит.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Грешка при изтриване на акаунта.";
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Нещо не е наред!");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Имейлът не е намерен!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Нещо се обърка. Опитайте отново.");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
