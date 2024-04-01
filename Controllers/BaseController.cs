using Microsoft.AspNetCore.Mvc;
using SFG.Data;

namespace SFG.Controllers
{
    public class BaseController : Controller
    {
        private readonly string ConnectionString = "Data Source=DASHBOARDPC;Initial Catalog=SFGDb;Persist Security Info=True;User ID=sa;Password=test@123;Multiple Active Result Sets=True;Trust Server Certificate=True";
        protected string GetConnection()
        {
            return ConnectionString;
        }

        protected readonly AppDbContext _db;

        public BaseController(AppDbContext dataBase)
        {
            _db = dataBase;
        }
    }
}
