using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models
{
    public class UpdateStatusDto
    {
        [Required]
        public string? Status { get; set; }
        [Required]
        public DateTime? ActualAuditDate { get; set; }
    }
}