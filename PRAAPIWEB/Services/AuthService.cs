using Newtonsoft.Json;
using PRAAPIWEB.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PRAAPIWEB.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client) // ← Должен быть public!
        {
            _client = client;
            _client.BaseAddress = new Uri("https://apipra-production-95b2.up.railway.app/");
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            var response = await _client.PostAsJsonAsync("api/Account/login", model);
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse>(json);
            if (result == null)
                throw new Exception("Invalid response format.");

            return result;
        }



        public async Task<bool> RegisterAsync(RegisterRequest model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Account/register", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ошибка регистрации: {response.StatusCode} - {error}");
                return false;
            }

            return true;
        }

        public async Task CreateUserAsync(User user)
        {
            var response = await _client.PostAsJsonAsync("api/Account/createuser", user);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"CreateUser failed: {error}");
            }
        }
    }
}
