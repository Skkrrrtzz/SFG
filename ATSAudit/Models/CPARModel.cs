using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
{
    public class CPARsModel
    {
        public CPARsModel(CPARDto response) {
            PlanId = response.PlanId;
            Respondent = response.Respondent;
            Requestor = response.Requestor;
            ResponseDueDate = response.ResponseDueDate;
            ISOClause = response.ISOClause;
            ProblemStatement = response.ProblemStatement;
        }

        public CPARsModel() {}

        [Key]
        public int CPARId { get; set; }
        [Required]
        public int? PlanId { get; set; }

        [Required]
        public string? Respondent { get; set; }
        [Required]
        public string? Requestor { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ResponseDueDate { get; set; }       
        // [Required]
        public string? ISOClause { get; set; }
        [Required]
        public string? ProblemStatement { get; set; }
        [Required]
        public string? PreparedBy { get; set; }
        public string? CheckedBy { get; set; }
        // public string CompanyDepartment { get; set; }
        public string? ApprovedBy { get; set; }

        // public string ModelPartNumber { get; set; }
        // public string LotBatchPONumber { get; set; }
        // public int DefectQuantity { get; set; }
        // public int AffectedQuantity { get; set; }

    }
}