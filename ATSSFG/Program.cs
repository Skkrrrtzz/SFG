using OfficeOpenXml;
using ATSSFG.Repository;
using ATSSFG.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<ISourcingRepository, SourcingRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<Emailing>();
// Authentication Cookies
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"\\DASHBOARDPC\ATSPortals\.cookies"))
    .SetApplicationName("SharedCookieApp");

builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        options.Cookie.Name = ".AspNet.SharedCookie";
        options.Cookie.Path = "/";
        options.SlidingExpiration = true;
        // options.Cookie.SameSite = SameSiteMode.None;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        // options.Cookie.Domain = "localhost"; // <-- REMOVE REMOOOOVEEEE
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.Headers["Rafols"] = "Gyatt";
                    context.Response.StatusCode = 401;
                }
                else
                {
                    context.Response.Redirect("https://localhost:7103/Login");
                }
                return Task.CompletedTask;
            }
        };
    });

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//builder.Services.AddSession(options => {
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();