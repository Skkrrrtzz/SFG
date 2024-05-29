using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models.Dto
{
    public class CorrectionDto
    {
        public int CPARId { get; set; }
        public string? CorrectionDescription { get; set; }
        public string? EscapeCause { get; set; }
        public string? Action { get; set; }
    }
}