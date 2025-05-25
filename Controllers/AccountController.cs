using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Security.Claims;

namespace PRAAPIWEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiUserService _userService;

        public AccountController(ApiUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Неверный email или пароль.";
            return RedirectToAction("Login");
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["Error"] = "Пользователь уже существует.";
                return RedirectToAction("Login");
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            bool success = await _userService.RegisterUserAsync(newUser);
            if (success)
            {
                TempData["Success"] = "Регистрация успешна.";
            }
            else
            {
                TempData["Error"] = "Ошибка при регистрации.";
            }

            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
