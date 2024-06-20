using APPLogin.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// //CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder =>
//         {
//             builder.AllowAnyOrigin()
//                     .AllowAnyMethod()
//                     .AllowAnyHeader();
//         });
// });

builder.Services.AddRazorPages();

builder.Services.AddTransient<ILoginRepository, LoginRepository>();

//Allow CORs
builder.Services.AddCors(options => 
    {
        options.AddPolicy(name: "_allowedOrigins",
            policy => 
            {
                policy.WithOrigins("https://localhost:44373", "https://localhost:7103");
            });
    });

// Authentication Cookies
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"\\DASHBOARDPC\\ATSPortals\.cookies"))
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

//TODO: Maybe implement shared session cookies so that sessions can end immediately after closing the app
// builder.Services.AddSession(options =>
// {
//     options.Cookie.Name = ".AspNet.SharedSession";
//     options.IdleTimeout = TimeSpan.FromMinutes(30);
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
//     options.Cookie.SameSite = SameSiteMode.None;
//     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
// });

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

// app.UseCors("_allowedOrigins");
//app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();