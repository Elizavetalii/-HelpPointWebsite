using Microsoft.AspNetCore.Authentication.Cookies;
using PRAAPIWEB.Services;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");
var baseUri = apiBaseUrl.EndsWith("/") ? apiBaseUrl : apiBaseUrl + "/";

// Регистрация HttpClient для AuthService
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(baseUri);
});

// Регистрация HttpClient для AboutProjectSectionService
builder.Services.AddHttpClient<AboutProjectSectionService>(client =>
{
    client.BaseAddress = new Uri(baseUri);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/account/accessdenied";
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient<ApiService>();
builder.Services.AddSingleton<LikeStorageService>();
//var likeStorage = new LikeStorageService();
//builder.Services.AddSingleton<LikeStorageService>(likeStorage);
//builder.Services.AddScoped<LikeStorageService>();
//builder.Services.AddSingleton<PRAAPIWEB.Services.LikeStorageService>(); 
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AboutProject}/{action=Index}/{id?}");

app.Run();
