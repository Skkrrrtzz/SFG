using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class MRPBOMProductModel
    {
        [Key]
        public int Id { get; set; }
        public string? Product { get; set; } = string.Empty;
        public string? PartNumber { get; set; } = string.Empty;
        [StringLength(4, MinimumLength = 1)]
        public string? Revision { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? DateModified { get; set; }
        public string? PreparedBy { get; set; } = string.Empty;
        public string? ReviewedBy { get; set; } = string.Empty;
    }
}
