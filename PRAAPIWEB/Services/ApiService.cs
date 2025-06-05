using PRAAPIWEB.Models;
using System.Text.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri("https://apipra-production-912b.up.railway.app/");
    }

    public async Task<List<Forumpost>> GetForumPostsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Forumpost>>("api/Forumposts");
    }

    public async Task<List<Forumreply>> GetForumRepliesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Forumreply>>("api/Forumreplies");
    }

    public async Task<List<Userquestion>> GetUserQuestionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Userquestion>>("api/Userquestions");
    }

    public async Task<List<Socialhelparticle>> GetSocialHelpArticlesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Socialhelparticle>>("api/Socialhelparticles");
    }
    public async Task<List<Medicalclinic>> GetMedicalClinicsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Medicalclinics");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Ответ API Medicalclinics: " + content);

            var clinics = System.Text.Json.JsonSerializer.Deserialize<List<Medicalclinic>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return clinics ?? new List<Medicalclinic>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при вызове GetMedicalClinicsAsync: " + ex);
            throw;
        }
    }
    public async Task<List<Medicalarticle>> GetMedicalArticlesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Medicalarticles");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Ответ API Medicalarticles: " + content);

            var clinics = System.Text.Json.JsonSerializer.Deserialize<List<Medicalarticle>>(content, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return clinics ?? new List<Medicalarticle>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при вызове Medicalarticles: " + ex);
            throw;
        }
    }
    public async Task<List<Migrationcenter>> GetMigrationCentersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Migrationcenters");

            // Логируем статус и содержимое ответа
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Content: {responseContent}");

            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<Migrationcenter>>(responseContent, options)
                   ?? new List<Migrationcenter>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMigrationCentersAsync: {ex}");
            throw;
        }
    }

    public async Task<List<Housingarticle>> GetHousingArticlesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Housingarticle>>("api/Housingarticles");
    }

    public async Task<List<Housingdocument>> GetHousingDocumentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Housingdocument>>("api/Housingdocuments");
    }

    public async Task<List<Housingarticletype>> GetHousingArticleTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Housingarticletype>>("api/Housingarticletypes");
    }
}
