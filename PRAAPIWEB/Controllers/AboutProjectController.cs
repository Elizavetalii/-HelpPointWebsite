using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PRAAPIWEB.Controllers
{
    public class AboutProjectController : Controller
    {
        private readonly AboutProjectSectionService _service;

        public AboutProjectController(AboutProjectSectionService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var sections = await _service.GetSectionsAsync();
            ViewBag.SectionsCount = sections?.Count ?? 0;

            return View("~/Views/Home/Index.cshtml", sections);
        }

    }

}
