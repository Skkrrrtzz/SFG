namespace COMLaptopPass.Models
{
    public class LaptopPassParameters
    {
        public string selecteditem { get; set; }
        public string password { get; set; }
        public string mode { get; set; }
        public string role { get; set; }
        public int userid { get; set; }

        public string bucode { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? stopdate { get; set; }
    }


    public class LaptopPassRoleModel
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
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

    public class LaptopPassRequestModel
    {
        public int lastid { get; set; }
        public int requestid { get; set; }
        public string bucode { get; set; }
        public string buname { get; set; }
        public int buid { get; set; }
        public string requestby { get; set; }
        public string localno { get; set; }
        public int userid { get; set; }
        public DateTime? requestdate { get; set; }
        public string name { get; set; }
        public string empno { get; set; }

        public string fapno { get; set; }

        public DateTime? startdate { get; set; }
        public DateTime? stopdate { get; set; }

        public string requeststatus { get; set; }

        public int statusid { get; set; }

        public int editmode { get; set; }

        public int empcode { get; set; }

        public byte[]? empbyte { get; set; }


    }

    public class LaptopPassSignatoriesModel
    {
        public string requestby { get; set; }
        public DateTime? requestdate { get; set; }

        public string approveby { get; set; }
        public DateTime? approvedate { get; set; }

        public string noteby { get; set; }
        public DateTime? notedate { get; set; }
    }

    public class LaptopPassEmployeeModel
    {
        public int empno { get; set; }
        public string empname { get; set; }
        public string fullname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
    }

    public class LaptopPassCommentModel
    {
        public int requestid { get; set; }
        public int commentid { get; set; }
        public string commenttext { get; set; }
        public string commentby { get; set; }
        public DateTime? commentdate { get; set; }
        public string middlename { get; set; }
    }

    public class LaptopPassApprovalModel
    {
        public int approvalcount { get; set; }
        public int approvalid { get; set; }
        public int requestid { get; set; }
        public string approvaltype { get; set; }

        public int userid { get; set; }
        public string username { get; set; }
        public DateTime? dateapproved { get; set; }
    }

    public class TutorialModel
    {
        public int manualid { get; set; }
        public string manualname { get; set; }
        public byte[] manualfile { get; set; }
    }
}
