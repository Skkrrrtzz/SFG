using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models
{
    public class CorrectiveRequestorModel
    {
        [Key]
        public string CARNumber { get; set; }
        public string Respondent { get; set; }
        // public string GroupSector { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ResponseDate { get; set; }       
        public string Requestor { get; set; }
        // public string CompanyDepartment { get; set; }
        public string ApprovedBy { get; set; }

        public string ProblemDescription { get; set; }
        public string ModelPartNumber { get; set; }
        // public string LotBatchPONumber { get; set; }
        // public int DefectQuantity { get; set; }
        // public int AffectedQuantity { get; set; }

    }
}