using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models.Dto
{
    public class UpdateStatusDto
    {
        public string Status { get; set; }
        public DateTime? ActualAuditDate { get; set; }
    }
}