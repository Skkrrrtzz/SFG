using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class AuditPlanModel
    {

        public AuditPlanModel() {}

        [Key]
        public int PlanId { get; set; }
        [Required]
        public string? Requestor { get; set; }
        [Required]
        public string? Department { get; set; } 
        [Required]
        public string? AuditCategory { get; set; } 
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public string? TimeEnd { get; set; }
        public DateTime? ActualAuditDate { get; set; }
        public bool AuditorApproved { get; set; }
        public bool AuditeeApproved { get; set; }
        [Required]
        public int Status { get; set; } 
        public string? Remark { get; set; }
        
    }

    enum AuditPlanStatus : int {
        ForApproval = 0,
        Open = 1,
        Closed = 2
    }
}

