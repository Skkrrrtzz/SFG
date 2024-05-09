using ATS_Library.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFG.Data;
using SFG.Models;
using System.Diagnostics;

namespace SFG.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(AppDbContext dataBase) : base(dataBase)
        {
        }

        private void CheckCookie()
        {
            HttpContext.Session.SetString("uniqueKey", HttpContext.Request.Cookies["uniqueKey"] == null ? "No Cookies" : HttpContext.Request.Cookies["uniqueKey"]);
        }

        public dynamic? GetSessionData()
        {
            dynamic sessionData = new ATS_Library.GetSession.Session().GetUserData("8114a571-c459-461a-9628-a19f0f052bfc");

            return sessionData;
        }

        //return new ATS_Library.GetSession.Session().GetUserData(HttpContext.Session.GetString("uniqueKey"));
        public IActionResult Checking()
        {
            CheckCookie();
            dynamic sessionData = GetSessionData();
            ViewBag.sessionName = sessionData?.AccountName;
            if (sessionData == null)
            {
                return Redirect("http://192.168.5.73:81");
            }

            return View();
        }

        public IActionResult Index()
        {
            // Call the Checking action method and return its result
            return Checking();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsersModel user)
        {
            dynamic ConnectionString = new ATS_Library.GetSession.Session().dashboardConnectionString;
            try
            {
                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Password == user.Password);

                if (existingUser != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Add an error message to ViewBag
                ViewBag.Message = "Invalid login attempt";
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"Login error: {ex.Message}");
                ViewBag.Message = $"Error: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("uniqueKey");
            return Redirect("http://192.168.5.73:81");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DisplayUsers()
        {
            Checking();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}