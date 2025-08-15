using ayhankizil.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsýný tanýmla
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

// Session servisini ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // 30 dakika timeout
    options.Cookie.HttpOnly = true;                  // XSS korumasý
    options.Cookie.IsEssential = true;               // GDPR uyumluluk
    options.Cookie.SameSite = SameSiteMode.Strict;   // CSRF korumasý - YENÝ EKLEME
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS zorunlu - YENÝ EKLEME
});

var app = builder.Build();

// HTTP pipeline yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Development'ta detaylý hata sayfasý
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'ini ekle (UseRouting'den sonra, UseAuthorization'dan önce)
app.UseSession();

app.UseAuthorization();

// Varsayýlan route ayarý
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();