using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class MRPBOMModel
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
        public int? Item { get; set; } = 0;
        public int? Level { get; set; } = 0;
        public string? PartNumberTable { get; set; } = string.Empty;
        public string? SAPPartNumber { get; set; } = string.Empty;
        public string? DescriptionTable { get; set; } = string.Empty;
        public string? Rev { get; set; } = string.Empty;
        public string? QPA { get; set; } = string.Empty;
        public string? EQPA { get; set; } = string.Empty;
        public string? UOM { get; set; } = string.Empty;
        public string? Commodity { get; set; } = string.Empty;
        public string? MPN { get; set; } = string.Empty;
        public string? Manufacturer { get; set; } = string.Empty;

    }
}
