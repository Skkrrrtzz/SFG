using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
{
    public class AuditPlanModel
    {
        public AuditPlanModel(AuditPlanDto response)
        {
            Requestor = response.Requestor;
            Department = response.Department;
            AuditCategory = response.AuditCategory;
            TargetDate = response.TargetDate;
            TimeEnd = response.TimeEnd;
            AuditorApproved = false;
            AuditeeApproved = false;
            Status = (int) AuditPlanStatus.ForApproval;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public AuditPlanModel() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Key]
        public int PlanId { get; set; }
        // public int Id { get; set; }
        [Required]
        public string Requestor { get; set; }
        [Required]
        public string Department { get; set; } 
        [Required]
        public string AuditCategory { get; set; } 
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public string TimeEnd { get; set; }
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

