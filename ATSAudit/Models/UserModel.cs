using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class UserModel
    {

        public UserModel() {}

        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set;}
        [Required]
        public string Password { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public bool isActive { get; set; }
        [Required]
        public bool Approver { get; set; }
        [Required]
        public bool Requestor { get; set; }
        [Required]
        public bool Respondent { get; set; }
        [Required]
        public bool Viewer { get; set; }
    }
}