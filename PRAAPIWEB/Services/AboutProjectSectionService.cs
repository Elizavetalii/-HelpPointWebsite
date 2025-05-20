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
            var response = await _client.GetAsync("api/Aboutprojectsections");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<Aboutprojectsection>>(json,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
