using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRepositories();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddRazorPages(options => 
{
    options.Conventions.AddPageRoute("/AuditPlans/DashboardRazor", "");
});

// Add Swagger and Controller Endpoints
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ATSAudit API",
        Version = "v1",
        Description = "API Documentation and Testing for PIMES-Web ATSAudit"
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 
}
);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\jrafols\Gits\PIMES-Web\.cookies"))
    .SetApplicationName("SharedCookieApp");

// Authentication Cookies
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        // options.SlidingExpiration = true;
        options.Cookie.Name = ".AspNet.SharedCookie";
        options.Cookie.Path = "/";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = true;
        // options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.Cookie.Domain = "localhost"; // <-- REMOVE REMOOOOVEEEE

    });

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

app.UseRouting();

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=auditplans}/{id?}/{whatever?}"
);
app.MapRazorPages();
app.UseSession();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
