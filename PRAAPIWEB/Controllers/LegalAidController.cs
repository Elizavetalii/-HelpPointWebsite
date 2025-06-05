using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PRAAPIWEB.Controllers
{
    public class LegalAidController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public LegalAidController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://apipra-production.up.railway.app/");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IActionResult> LegalHelp(string searchQuery)
        {
            try
            {
                var articles = await GetLegalArticlesAsync();

                // Применяем поиск, если есть запрос
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    articles = articles.Where(a =>
                        (a.Title?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (a.Content?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                // Получаем уникальные категории из статей
                var categories = articles
                    .Select(a => a.Category)
                    .Distinct()
                    .ToList();

                var viewModel = new LegalHelpViewModel
                {
                    LegalArticles = articles,
                    Categories = categories,
                    SearchQuery = searchQuery
                };

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                return View(new LegalHelpViewModel
                {
                    ErrorMessage = "Не удалось загрузить юридические материалы. Пожалуйста, попробуйте позже."
                });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(string title, string content, string category)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var newArticle = new Legalarticle
            {
                Title = title,
                Content = content,
                Category = category
            };

            var response = await _httpClient.PostAsJsonAsync("api/Legalarticles", newArticle);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("LegalHelp");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var response = await _httpClient.DeleteAsync($"api/Legalarticles/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction("LegalHelp");
        }

        private async Task<List<Legalarticle>> GetLegalArticlesAsync()
        {
            var response = await _httpClient.GetAsync("api/Legalarticles");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Legalarticle>>(stream, _jsonOptions) ?? new List<Legalarticle>();
        }
    }
}