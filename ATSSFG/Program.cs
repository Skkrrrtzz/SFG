using OfficeOpenXml;
using ATSSFG.Repository;
using ATSSFG.Services;

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
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ISourcingRepository, SourcingRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<Emailing>();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();