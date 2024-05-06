using WEBTemplate.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<ILaptopPassRepository, LaptopPassRepository>();


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
