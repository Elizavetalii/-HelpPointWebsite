using Microsoft.AspNetCore.Authentication.Cookies;
using PRAAPIWEB.Services;

var builder = WebApplication.CreateBuilder(args);

// Читаем базовый URL из настроек
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");

// Регистрируем HttpClient для AboutProjectSectionService
builder.Services.AddHttpClient<AboutProjectSectionService>(client =>
{
    var baseUri = apiBaseUrl.EndsWith("/") ? apiBaseUrl : apiBaseUrl + "/";
    client.BaseAddress = new Uri(baseUri);
});

// Добавляем аутентификацию через куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // если не авторизован — перекинет на эту страницу
    });

builder.Services.AddHttpClient<ApiUserService>();

// Регистрируем MVC
builder.Services.AddControllersWithViews();

// ← Здесь создаем объект app
var app = builder.Build();

// Конфигурация HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AboutProject}/{action=Index}/{id?}");

app.Run();
