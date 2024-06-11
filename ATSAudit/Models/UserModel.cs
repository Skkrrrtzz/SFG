using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class UserModel
    {

        public UserModel() {}

        [Key]
        public int UserId { get; set; }
        Name nvarchar(max) NOT NULL,
        Email nvarchar(max) NOT NULL,
        Password nvarchar(max) NULL,
        Encoder bit NULL,
        Processor bit NULL,
        Viewer bit NULL,
        Admin bit NULL,
        Department 
        IsActive 
        
    }
}