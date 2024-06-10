using System.ComponentModel.DataAnnotations;

namespace QA_Audit_Fresh.Models
{
    public class CorrectionModel
    {
        public CorrectionModel() {}

        [Key]
        public int CorrectionId { get; set; }
        [Required]
        public int CPARId { get; set; }
        [Required]
        public string? CorrectionDescription { get; set; }
        [Required]
        public string? EscapeCause { get; set; }
        [Required]
        public string? Action { get; set; }
    }
}