using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class CPARModel
    {
        public CPARModel() {}

        [Key]
        public int CPARId { get; set; }
        [Required]
        public int? PlanId { get; set; }
        [Required]
        public string? Respondent { get; set; }
        [Required]
        public string? Requestor { get; set; }
        [Required]
        public DateTime? IssueDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        [Required]
        public DateTime? ResponseDueDate { get; set; }       
        [Required]
        public string? ISOClause { get; set; }
        [Required]
        public string? ProblemStatement { get; set; }
        public string? PreparedBy { get; set; }
        public string? CheckedBy { get; set; }
        // public string CompanyDepartment { get; set; }
        public string? ApprovedBy { get; set; }

        //Part of the CPARsActualAuditDate View
        public DateTime? ActualAuditDate { get; set; }

        // public string ModelPartNumber { get; set; }
        // public string LotBatchPONumber { get; set; }
        // public int DefectQuantity { get; set; }
        // public int AffectedQuantity { get; set; }

    }
}