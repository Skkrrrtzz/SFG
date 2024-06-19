using System.ComponentModel.DataAnnotations;

namespace ATSAudit.Models
{
    public class PreventiveActionModel
    {

        public PreventiveActionModel() {}

        [Key]
        public int PreventiveActionId { get; set; }
        [Required]
        public int CPARId { get; set; }
        [Required]
        public string? PreventiveActionDescription { get; set; }
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public string? Responsible  { get; set; }
        public byte Status { get; set; }
        public string? EvidenceFiles { get; set; }

    }
}