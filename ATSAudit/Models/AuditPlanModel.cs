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
        // public AuditPlanModel(string requestor, string department, string auditCategory, DateTime targetDate, string timeEnd)
        // {
        //     Requestor = requestor;
        //     Department = department;
        //     AuditCategory = auditCategory;
        //     TargetDate = targetDate;
        //     TimeEnd = timeEnd;
        //     Status = StatusEnum.Open.ToString();
        // }

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

        public AuditPlanModel() {}
        
        [Key]
        public int PlanId { get; set; }
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

