using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class CorrectiveActionModel
    {


        public CorrectiveActionModel() {}

        [Key]
        public int CorrectiveActionId { get; set; }
        [Required]
        public int CPARId { get; set; }
        [Required]
        public string? CorrectiveActionDescription { get; set; }
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public string? Responsible  { get; set; }
        public byte Status { get; set; } = 0;
    }
}