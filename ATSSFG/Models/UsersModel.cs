using System.ComponentModel.DataAnnotations;

namespace ATSSFG.Models
{
    public class UsersModel
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }
    }

    public class DepartmentEmails
    {
        public static string SourcingEmail = "atssourcing@pimes.com.ph";
        public static string BusinessOpsEmail = "atsbusinessops@pimes.com.ph";
    }
}