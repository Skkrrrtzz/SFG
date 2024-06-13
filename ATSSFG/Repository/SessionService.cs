using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public interface ISessionService
    {
        void SetRole(string strTitle, UsersInfoModel user);
        //string GetAppName();
        //string GetUserName();
        //bool GetAdmin();
        //bool GetEncoder();
        //bool GetProcessor();
        //bool GetViewer();
        //string GetDepartment();

    }

    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetRole(string strTitle, UsersInfoModel user)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("AppName", strTitle);
            session.SetString("UserName", user.Name);
            session.SetString("UserEmail", user.Email);
            session.SetString("Encoder", user.Encoder.ToString());
            session.SetString("Processor", user.Processor.ToString());
            session.SetString("Viewer", user.Viewer.ToString());
            session.SetString("Admin", user.Admin.ToString());
            session.SetString("Dept", user.Department);
        }
        //public string GetAppName()
        //{
        //    return _httpContextAccessor.HttpContext.Session.GetString("AppName") ?? "";
        //}

        //public string GetUserName()
        //{
        //    return _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? "";
        //}

        //public bool GetAdmin()
        //{
        //    string admin = _httpContextAccessor.HttpContext.Session.GetString("Admin") ?? "";
        //    return admin.Equals("True", StringComparison.OrdinalIgnoreCase);
        //}
        //public string GetDepartment()
        //{
        //    return _httpContextAccessor.HttpContext.Session.GetString("Department") ?? "";
        //}
        //public bool GetEncoder()
        //{
        //    string encoder = _httpContextAccessor.HttpContext.Session.GetString("Encoder") ?? "";
        //    return encoder.Equals("True", StringComparison.OrdinalIgnoreCase);
        //}
        //public bool GetProcessor()
        //{
        //    string processor = _httpContextAccessor.HttpContext.Session.GetString("Processor") ?? "";
        //    return processor.Equals("True", StringComparison.OrdinalIgnoreCase);
        //}
        //public bool GetViewer()
        //{
        //    string viewer = _httpContextAccessor.HttpContext.Session.GetString("Viewer") ?? "";
        //    return viewer.Equals("True", StringComparison.OrdinalIgnoreCase);
        //}
    }

}
