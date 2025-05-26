using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;

namespace PRAAPIWEB.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        // Вход - принимает данные из формы (application/x-www-form-urlencoded)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Некорректные данные.");

            try
            {
                var loginResult = await _authService.LoginAsync(model);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResult.User.Email),
                    new Claim("UserId", loginResult.User.Id.ToString()),
                    new Claim("UserName", loginResult.User.Name)
                    // можно добавить и другие claim по необходимости
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Json(new { success = true, message = loginResult.Message });

            }
            catch (Exception ex)
            {
                // Возвращаем ошибку в JSON с сообщением
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }


        // Регистрация - данные тоже из формы

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest model)

        {
            if (model == null)
                return BadRequest("Модель регистрации не получена");

            Console.WriteLine($"Register request: Name={model.Name}, Email={model.Email}");

            try
            {
                await _authService.RegisterAsync(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, $"Ошибка регистрации: {ex.Message}");
            }

        }



        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
