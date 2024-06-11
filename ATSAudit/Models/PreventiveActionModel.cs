using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
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

    }
}