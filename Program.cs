
using OfficeOpenXml;
using SFG.Repository;
using SFG.Services;

namespace SFG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ISourcingRepository, SourcingRepository>();
            builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<UploadService>();
            builder.Services.AddScoped<Emailing>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            builder.Services.AddSession(options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Dashboard}/{id?}");

            app.Run();
        }
    }
}
