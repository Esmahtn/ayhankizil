using ayhankizil.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�n� tan�mla
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

// Session servisini ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // 30 dakika timeout
    options.Cookie.HttpOnly = true;                  // XSS korumas�
    options.Cookie.IsEssential = true;               // GDPR uyumluluk
    options.Cookie.SameSite = SameSiteMode.Strict;   // CSRF korumas� - YEN� EKLEME
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS zorunlu - YEN� EKLEME
});

var app = builder.Build();

// HTTP pipeline yap�land�rmas�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Development'ta detayl� hata sayfas�
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'ini ekle (UseRouting'den sonra, UseAuthorization'dan �nce)
app.UseSession();

app.UseAuthorization();

// Varsay�lan route ayar�
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();