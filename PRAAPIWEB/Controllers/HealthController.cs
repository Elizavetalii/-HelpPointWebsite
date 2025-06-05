using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRAAPIWEB.Controllers
{
    public class HealthController : Controller
    {
        private readonly ApiService _apiService;

        public HealthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Medical()
        {
            // Get medical clinics and articles from API
            var clinicsTask = _apiService.GetMedicalClinicsAsync();
            var articlesTask = _apiService.GetMedicalArticlesAsync();

            // Ожидаем завершения обоих задач параллельно
            await Task.WhenAll(clinicsTask, articlesTask);

            // Prepare emergency contacts
            var emergencyContacts = new List<EmergencyContact>
            {
                new EmergencyContact { Name = "Общий номер службы экстренной помощи", Number = "112" },
                new EmergencyContact { Name = "Полиция", Number = "102" },
                new EmergencyContact { Name = "Пожарная служба", Number = "101" },
                new EmergencyContact { Name = "Скорая медицинская помощь", Number = "103" }
            };

            // Prepare emergency procedures
            var emergencyProcedures = new List<EmergencyProcedure>
            {
                new EmergencyProcedure { Title = "Что делать в случае тяжелой травмы", Completed = true },
                new EmergencyProcedure { Title = "Вызов скорой помощи", Completed = true },
                new EmergencyProcedure { Title = "Как справиться с внезапной сильной болью", Completed = true },
                new EmergencyProcedure { Title = "Распознавание симптомов инсульта", Completed = true }
            };

            // Pass data to view
            ViewBag.EmergencyContacts = emergencyContacts;
            ViewBag.EmergencyProcedures = emergencyProcedures;

            var model = new MedicalViewModel
            {
                Clinics = await clinicsTask,
                Articles = await articlesTask
            };

            return View(model);
        }


    }
}