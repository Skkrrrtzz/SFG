using System.ComponentModel.DataAnnotations;

namespace ATSAudit.Models
{
    public class DepartmentModel    {

        public DepartmentModel(string departmentName, string departmentManager)
        {
            DepartmentName = departmentName;
            DepartmentManager = departmentManager;
        }

        [Key]
        public int DepartmentID { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        public string DepartmentManager { get; set; }
    }
}