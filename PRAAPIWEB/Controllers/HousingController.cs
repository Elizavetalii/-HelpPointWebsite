using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRAAPIWEB.Controllers
{
    public class HousingController : Controller
    {
        private readonly ApiService _apiService;

        public HousingController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Accommodation()
        {
            // Параллельно загружаем все необходимые данные
            var articlesTask = _apiService.GetHousingArticlesAsync();
            var documentsTask = _apiService.GetHousingDocumentsAsync();
            var articleTypesTask = _apiService.GetHousingArticleTypesAsync();

            await Task.WhenAll(articlesTask, documentsTask, articleTypesTask);

            var articles = await articlesTask;
            var documents = await documentsTask;
            var articleTypes = await articleTypesTask;

            // Сопоставляем ArticleType с статьями
            foreach (var article in articles)
            {
                if (article.TypeId.HasValue)
                {
                    article.ArticleType = articleTypes.FirstOrDefault(t => t.Id == article.TypeId);
                }
            }

            // Словари с дополнительными ресурсами
            var articleImages = new Dictionary<int, string>
            {
                { 1, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202025-06-05%20030424.png?raw=true" },
                { 2, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202025-06-05%20030436.png?raw=true" },
                { 3, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202025-06-05%20030453.png?raw=true" },
                { 4, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202025-06-05%20030500.png?raw=true" },

            };

            var articleLinks = new Dictionary<int, List<LinkArticle>>
            {
                { 2, new List<LinkArticle> {
                    new() { Title = "Официальный сайт МВД", Url = "https://мвд.рф" },
                    new() { Title = "Миграционный учет", Url = "https://гувм.мвд.рф" }
                }}
            };
            var model = new HousingViewModel
            {
                Articles = articles,
                Documents = documents,
                BasicInfoArticles = articles.Where(a => a.TypeId == 2).ToList(), // ID для основной информации
                UsefulResourcesArticles = articles.Where(a => a.TypeId == 1).ToList(), // ID для полезных ресурсов
                DocumentArticles = articles.Where(a => a.TypeId == 3).ToList(), // ID для документов
                ArticleTypes = articleTypes,
                ArticleImages = articleImages,
                ArticleLinks = articleLinks
            };

            return View(model);
        }
    }
}