using Microsoft.AspNetCore.Mvc;

namespace PRAAPIWEB.Controllers
{
    public class AboutPrController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
