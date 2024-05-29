using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
{
    public class CorrectionModel
    {
        public CorrectionModel(CorrectionDto response) {
            CPARId = response.CPARId;
            CorrectionDescription = response.CorrectionDescription;
            EscapeCause = response.EscapeCause;
            Action = response.Action;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public CorrectionModel() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Key]
        public int CorrectionId { get; set; }
        public int CPARId { get; set; }
        public string CorrectionDescription { get; set; }
        public string EscapeCause { get; set; }
        public string Action { get; set; }
    }
}