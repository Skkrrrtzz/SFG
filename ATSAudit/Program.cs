// using QA_Audit_Fresh.Controllers.Repositories;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddRepositories();

// Add services to the container.
// builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Swagger for API testing
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.UseSwagger();
// app.UseSwaggerUI();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "api",
    pattern: "api/{action=auditplans}/{id?}/{whatever?}");

app.MapRazorPages();

app.Run();
