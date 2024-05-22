using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models
{
    public class CorrectiveRespondentModel
    {
        [Key]
        public string CARNumber { get; set; }
        public string Correction { get; set; } //Responsible, Implementation
        public string EscapeCause { get; set; } // Action, Responsible, Implementation
        // public string ActionEscape { get; set; }
        // public string EscapeResponsible { get; set; }
        // public string EscapeCauseImplementation { get; set; }
        public string CauseAnalysis { get; set; }
        public string RootCause { get; set; }
        
        public string CorrectiveActionRoot { get; set; } // Responsible, Implementation
        public string Standardization { get; set; }

    }
}