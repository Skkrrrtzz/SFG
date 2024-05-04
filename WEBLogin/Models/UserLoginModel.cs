namespace WEBLogin.Models
{
    public class UserLoginParameters
    {

        public string p_pass { get; set; }

        public string selecteditem { get; set; }
        public string password { get; set; }
        public string mode { get; set; }
        public string role { get; set; }
        public int userid { get; set; }
        public string bucode { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? stopdate { get; set; }
    }


    public class UserLoginModel
    {
        public string username { get; set; }
        public string employeeno { get; set; }
        public string email { get; set; }
        public string localno { get; set; }
        public string password { get; set; }
        public int applaptopass { get; set; }
        public int appitjo { get; set; }
        public int appfacilityjo { get; set; }
    }
}
