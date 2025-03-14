using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Yaz�l�mOrg15Eylul_ETicaret.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "15EylulEticaret.Auth";
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";

    });


builder.Services.AddSession(option =>
{
    //biz ekledik 1 dk s�re olarak ayarlad�k
    option.IOTimeout = TimeSpan.FromMinutes(10);
});



//Alert t�rk�e karakter sorunun ��zmek i�in ��zmek i�in  koyduk
builder.Services.AddWebEncoders(o =>
{
    o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

//biz ekledik layouta session login g�r�n�m� i�in
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();//biz ekledik 

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapHub<AdminHub>("/adminHub");

app.Run();
