using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRAAPIWEB.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using PRAAPIWEB.Services; // если сервис лежит в другом namespace
namespace PRAAPIWEB.Controllers
{
    public class SupportController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly LikeStorageService _likeStorage;

        public SupportController(IHttpClientFactory httpClientFactory, LikeStorageService likeStorage)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://apipra-production.up.railway.app/");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _likeStorage = likeStorage;
        }

        public async Task<IActionResult> Forum(string searchQuery)
        {
            try
            {
                var postsTask = GetForumPostsAsync();
                var repliesTask = GetForumRepliesAsync();
                var questionsTask = GetUserQuestionsAsync();
                var usersTask = GetUsersAsync();

                await Task.WhenAll(postsTask, repliesTask, questionsTask, usersTask);

                var posts = await postsTask;
                var replies = await repliesTask;
                var users = await usersTask;

                // Применяем поиск, если есть запрос
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    posts = posts.Where(p =>
                        p.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        p.Content.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var viewModel = new ForumViewModel
                {
                    ForumPosts = posts,
                    ForumReplies = replies,
                    UserQuestions = await questionsTask,
                    Users = users,
                    SearchQuery = searchQuery
                };

                // Добавляем информацию о лайках пользователя в ViewBag
                if (User.Identity.IsAuthenticated && int.TryParse(User.FindFirstValue("UserId"), out int userId))
                {
                    ViewBag.UserLikes = posts.ToDictionary(
                        p => p.Id,
                        p => _likeStorage.HasUserLiked(p.Id, userId));
                }

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                return View(new ForumViewModel
                {
                    ErrorMessage = "Не удалось загрузить данные форума. Пожалуйста, попробуйте позже."
                });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(string title, string content)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            if (!int.TryParse(User.FindFirstValue("UserId"), out int userId))
                return Challenge();

            var newPost = new Forumpost
            {
                Title = title,
                Content = content,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            var response = await _httpClient.PostAsJsonAsync("api/Forumposts", newPost);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Forum");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReply(int postId, string content)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            if (!int.TryParse(User.FindFirstValue("UserId"), out int userId))
                return Challenge();

            var newReply = new Forumreply
            {
                PostId = postId,
                Content = content,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            var response = await _httpClient.PostAsJsonAsync("api/Forumreplies", newReply);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Forum");
        }

        [Authorize]
        [HttpPost]
        public IActionResult LikePost(int postId)
        {
            Console.WriteLine($"LikePost called for postId: {postId}"); // или используйте ILogger

            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User not authenticated");
                return Json(new { success = false, message = "Не авторизован" });
            }

            if (!int.TryParse(User.FindFirstValue("UserId"), out int userId))
            {
                Console.WriteLine("Failed to parse user ID");
                return Json(new { success = false, message = "Ошибка идентификации пользователя" });
            }

            var isLiked = _likeStorage.ToggleLike(postId, userId);
            var likesCount = _likeStorage.GetLikesCount(postId);

            Console.WriteLine($"Like toggled - postId: {postId}, userId: {userId}, isLiked: {isLiked}, count: {likesCount}");

            return Json(new
            {
                success = true,
                likesCount,
                isLiked
            });
        }

        private async Task<List<Forumpost>> GetForumPostsAsync()
        {
            var response = await _httpClient.GetAsync("api/Forumposts");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Forumpost>>(stream, _jsonOptions) ?? new List<Forumpost>();
        }

        private async Task<List<Forumreply>> GetForumRepliesAsync()
        {
            var response = await _httpClient.GetAsync("api/Forumreplies");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Forumreply>>(stream, _jsonOptions) ?? new List<Forumreply>();
        }

        private async Task<List<Userquestion>> GetUserQuestionsAsync()
        {
            var response = await _httpClient.GetAsync("api/Userquestions");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Userquestion>>(stream, _jsonOptions) ?? new List<Userquestion>();
        }

        private async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/Users");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<User>>(stream, _jsonOptions) ?? new List<User>();
        }
    }

}