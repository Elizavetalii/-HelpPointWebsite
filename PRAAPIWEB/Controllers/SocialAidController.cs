using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using PRAAPIWEB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PRAAPIWEB.Controllers
{
    public class SocialAidController : Controller
    {
        private readonly ApiService _apiService;

        public SocialAidController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> SocialSupport()
        {
            // Получаем статьи из API
            var articles = await _apiService.GetSocialHelpArticlesAsync();

            // Словарь с URL изображений по Id статьи
            var imageUrls = new Dictionary<int, string>
    {
        { 1, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/%D0%A1%D0%BD%D0%B8%D0%BC%D0%BE%D0%BA%20%D1%8D%D0%BA%D1%80%D0%B0%D0%BD%D0%B0%202025-05-29%20185146.png?raw=true" },
        { 2, "https://github.com/Elizavetalii/-HelpPointWebsite/blob/main/family.jpg?raw=true" }    };

            // Расширенный словарь с полезными ссылками
            var articleLinks = new Dictionary<int, List<ArticleLink>>
    {
        {
            1, // Помощь мигрантам в России
            new List<ArticleLink>
            {
                new ArticleLink
                {
                    Title = "Миграционный центр МВД",
                    Url = "https://мвд.рф/mvd/structure1/Glavnie_upravlenija/guvm",
                    Icon = "bi bi-passport",
                    Description = "Официальный портал по вопросам миграции"
                },
                new ArticleLink
                {
                    Title = "Горячая линия для мигрантов",
                    Url = "tel:+78007007999",
                    Icon = "bi bi-telephone",
                    Description = "Круглосуточная бесплатная поддержка"
                },
                new ArticleLink
                {
                    Title = "Центры правовой помощи",
                    Url = "https://migrant.ru/centers",
                    Icon = "bi bi-journal-text",
                    Description = "Адреса юридических консультаций в вашем городе"
                },
                new ArticleLink
                {
                    Title = "Курсы русского языка",
                    Url = "https://pushkininstitute.ru/external_courses",
                    Icon = "bi bi-translate",
                    Description = "Бесплатные онлайн-курсы от Института Пушкина"
                },
                new ArticleLink
                {
                    Title = "Трудоустройство мигрантов",
                    Url = "https://trudvsem.ru/info/migrant",
                    Icon = "bi bi-briefcase",
                    Description = "Официальный портал поиска работы"
                }
            }
        },
        {
            2, // Поддержка семей мигрантов
            new List<ArticleLink>
            {
                new ArticleLink
                {
                    Title = "Социальная защита семей",
                    Url = "https://rosmintrud.ru/social/family",
                    Icon = "bi bi-people",
                    Description = "Программы поддержки семей с детьми"
                },
                new ArticleLink
                {
                    Title = "Медицинская помощь детям",
                    Url = "https://www.rosminzdrav.ru/poleznye-resursy/migrantam",
                    Icon = "bi bi-heart-pulse",
                    Description = "Как получить медицинскую страховку"
                },
                new ArticleLink
                {
                    Title = "Школы для детей мигрантов",
                    Url = "https://edu.gov.ru/activity/main_activities/adaptation_of_migrants",
                    Icon = "bi bi-book",
                    Description = "Информация о зачислении в школы"
                },
                new ArticleLink
                {
                    Title = "Кризисные центры",
                    Url = "https://www.rus-immigrant.ru/help",
                    Icon = "bi bi-house-heart",
                    Description = "Помощь в сложных жизненных ситуациях"
                },
                new ArticleLink
                {
                    Title = "Культурные центры",
                    Url = "https://cultural.ru/migrant",
                    Icon = "bi bi-globe2",
                    Description = "Адаптация и межкультурные программы"
                },
                new ArticleLink
                {
                    Title = "Бесплатная психологическая помощь",
                    Url = "tel:88002000122",
                    Icon = "bi bi-chat-square-text",
                    Description = "Горячая линия психологической поддержки"
                }
            }
        }
    };

            ViewBag.ImageUrls = imageUrls;
            ViewBag.ArticleLinks = articleLinks;
            return View(articles);
        }
    }
}
