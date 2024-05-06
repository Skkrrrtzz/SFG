namespace WEBTemplate.Models
{   
    public class UserLoginModel
    {
        public string username { get; set; }
        public string empno { get; set; }
        public int appaccess { get; set; }
    }


    public class UserRoleModel
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string localno { get; set; }
        public int buid { get; set; }
        public string bucode { get; set; }
        public string buname { get; set; }
        public int requestor { get; set; }
        public int approver { get; set; }
        public int noter { get; set; }
        public int isactive { get; set; }
    }
}
