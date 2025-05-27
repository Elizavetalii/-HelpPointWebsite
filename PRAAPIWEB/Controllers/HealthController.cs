using Microsoft.AspNetCore.Mvc;

namespace PRAAPIWEB.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Medical()
        {
            return View();
        }
    }
}
