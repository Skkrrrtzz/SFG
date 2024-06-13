using System.ComponentModel.DataAnnotations;

namespace ATSSFG.Models
{
    public class UsersInfoModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool? Encoder { get; set; }
        public bool? Processor { get; set; }
        public bool? Viewer { get; set; }
        public bool? Admin { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class DepartmentEmails
    {
        public static string SourcingEmail = "atssourcing@pimes.com.ph";
        public static string BusinessOpsEmail = "atsbusinessops@pimes.com.ph";
    }
}