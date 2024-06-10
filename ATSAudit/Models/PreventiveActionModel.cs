using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class PreventiveActionModel
    {

        public PreventiveActionModel() {}

        [Key]
        public int PreventiveActionId { get; set; }
        public int CPARId { get; set; }
        public string? PreventiveActionDescription { get; set; }
        public DateTime? TargetDate { get; set; }
        public string? Responsible  { get; set; }

    }
}