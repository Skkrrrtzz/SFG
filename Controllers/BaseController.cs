using Microsoft.AspNetCore.Mvc;

namespace SFG.Controllers
{
    public class BaseController : Controller
    {
        private readonly string ConnectionString = "Data Source=DASHBOARDPC;Initial Catalog=SFGDb;Persist Security Info=True;User ID=sa;Password=test@123;Multiple Active Result Sets=True;Trust Server Certificate=True";

        protected string GetConnection()
        {
            return ConnectionString;
        }
    }
}