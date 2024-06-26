
using Microsoft.AspNetCore.Mvc;
using SFG.Models;
using System.Diagnostics;

namespace SFG.Controllers
{
    public class HomeController : BaseController
    {
        private void CheckCookie()
        {
            HttpContext.Session.SetString("uniqueKey", HttpContext.Request.Cookies["uniqueKey"] == null ? "No Cookies" : HttpContext.Request.Cookies["uniqueKey"]);
        }

        public dynamic? GetSessionData()
        {
            dynamic sessionData = new ATS_Library.GetSession.Session().GetUserData("cbe872e4-985d-49a1-896d-4d206a7efe9d");

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
        //public async Task<IActionResult> Login(UsersModel user)
        //{
        //    dynamic ConnectionString = new ATS_Library.GetSession.Session().dashboardConnectionString;
        //    try
        //    {
        //        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Password == user.Password);

        //        if (existingUser != null)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        // Add an error message to ViewBag
        //        ViewBag.Message = "Invalid login attempt";
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it accordingly
        //        Console.WriteLine($"Login error: {ex.Message}");
        //        ViewBag.Message = $"Error: {ex.Message}";
        //        return View();
        //    }
        //}

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