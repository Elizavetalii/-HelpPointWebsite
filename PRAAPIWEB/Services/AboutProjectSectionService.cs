using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using PRAAPIWEB.Models;

namespace PRAAPIWEB.Services
{
    public class AboutProjectSectionService
    {
        private readonly HttpClient _client;

        public AboutProjectSectionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Aboutprojectsection>> GetSectionsAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/Aboutprojectsections");
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Ошибка API: {json}");

                return System.Text.Json.JsonSerializer.Deserialize<List<Aboutprojectsection>>(json,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении данных: " + ex.Message);
                throw; // или верни пустой список, если критично
            }
        }

    }
}
