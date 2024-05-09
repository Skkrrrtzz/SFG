namespace APPLogin.Models
{
    public class UserLoginModel
    {
        public string password { get; set; }
        public string username { get; set; }
        public string employeeno { get; set; }
        public int appaccess { get; set; }
    }

    public class UserMenuModel
    {
        public string menuitem { get; set; }
        public string menuprogram { get; set; }
    }

    public class UserPendingModel
    {
        public string modulename { get; set; }
        public string bucode { get; set; }
        public string rolename { get; set; }
        public string username { get; set; }
        public int pendingcount { get; set; }
        public int buid { get; set; }
        public int userid { get; set; }
        public string email { get; set; }
        public string localno { get; set; }
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