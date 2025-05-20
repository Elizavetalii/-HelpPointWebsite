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

// Регистрируем MVC
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AboutProject}/{action=Index}/{id?}");

app.Run();
