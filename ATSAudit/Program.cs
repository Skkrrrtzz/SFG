// using QA_Audit_Fresh.Controllers.Repositories;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories();

// Add services to the container.
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller=auditplans}/{id?}/{whatever?}"
);
app.MapRazorPages();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
