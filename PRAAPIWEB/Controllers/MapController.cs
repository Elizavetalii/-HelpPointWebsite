using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRAAPIWEB.Controllers
{
    public class MapController : Controller
    {
        private readonly ApiService _apiService;

        public MapController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> HelpMap()
        {
            var clinics = await _apiService.GetMedicalClinicsAsync();
            var centers = await _apiService.GetMigrationCentersAsync();

            // Словарь с координатами по адресам
            var locationCoordinates = new Dictionary<string, (double Lat, double Lng)>
    {
        { "г. Москва, ул. Лесная, д. 57", (55.781823, 37.593201) },  // Координаты для поликлиники №220
        { "г. Москва, ул. Тверская, д. 12", (55.763305, 37.609371) }  // Координаты для миграционного центра
    };

            var locations = new List<HelpLocationViewModel>();

            locations.AddRange(clinics.Select(c => new HelpLocationViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone,
                Region = c.Region,
                Type = "clinic",
                IsFree = c.IsFree,
                // Получаем координаты из словаря или используем значения по умолчанию
                Latitude = locationCoordinates.TryGetValue(c.Address, out var coords) ? coords.Lat : 55.751244,
                Longitude = locationCoordinates.TryGetValue(c.Address, out coords) ? coords.Lng : 37.618423
            }));

            locations.AddRange(centers.Select(m => new HelpLocationViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Address = m.Address,
                Phone = m.Phone,
                Region = m.Region,
                Type = "migration",
                WorkingHours = m.WorkingHours,
                // Получаем координаты из словаря или используем значения по умолчанию
                Latitude = locationCoordinates.TryGetValue(m.Address, out var coords) ? coords.Lat : 55.751244,
                Longitude = locationCoordinates.TryGetValue(m.Address, out coords) ? coords.Lng : 37.618423
            }));

            return View(locations);
        }
    }
}
