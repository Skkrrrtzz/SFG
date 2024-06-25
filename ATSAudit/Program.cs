using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using ATSAudit;

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
    options.Conventions.AddPageRoute("/AuditPlans/Index", "");
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

//Allow CORs
// builder.Services.AddCors(options => 
//     {
//         options.AddPolicy(name: "_allowedOrigins",
//             policy => 
//             {
//                 policy.WithOrigins("https://localhost:5150", "https://localhost:7103");
//             });
//     });


// Authentication Cookies
    //TODO: Change Cookies to Persist in Users' AppData rather than the Server!!
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"\\DASHBOARDPC\\ATSPortals\.cookies"))
    .ProtectKeysWithDpapi(protectToLocalMachine: true)
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
                    context.Response.Headers["The-Rizzler"] = "Mewing";
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                }
                else
                {
                    context.Response.Redirect("https://localhost:7103/Login");
                    return Task.CompletedTask;
                }

            }

        };
    });

// TODO: Maybe implement shared session cookies so that sessions can end immediately after closing the app
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

app.UseRouting();

// app.UseCors("_allowedOrigins");
// app.UseMiddleware<UnauthorizedRedirectMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// app.UseSession();

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=auditplans}/{id?}/{whatever?}"
);

app.MapRazorPages();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
