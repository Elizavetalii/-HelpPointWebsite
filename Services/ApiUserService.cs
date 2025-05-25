using PRAAPIWEB.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace PRAAPIWEB.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiUserService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<User>($"{_baseUrl}api/users/email/{email}");
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/users", user);
            return response.IsSuccessStatusCode;
        }
    }
}
