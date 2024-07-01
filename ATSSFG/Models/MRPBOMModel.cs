using System.ComponentModel.DataAnnotations;

namespace ATSSFG.Models
{
    public class MRPBOMModel
    {
        [Key]
        public int Id { get; set; }

        public string? Product { get; set; } = string.Empty;
        public string? PartNumber { get; set; } = string.Empty;
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
        public string? UploadedBy { get; set; } = string.Empty;
    }
    public class QuotationModel
    {
        [Key]
        public int Id { get; set; }

        public string PartNumber { get; set; }
    }
    public class LastPurchaseInfoModel
    {
        [Key]
        public int Id { get; set; }

        public string? ItemNo { get; set; }
        public string? ForeignName { get; set; }
        public string? ItemDescription { get; set; }

        [StringLength(4, MinimumLength = 1)]
        public string? Unit { get; set; }

        public decimal? GWRLQty { get; set; }
        public DateTime? LastPurchasedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.####}", ApplyFormatInEditMode = true)]
        public decimal? LastPurchasedUSDPrice { get; set; }

        public string? CustomerVendorCode { get; set; }
        public string? CustomerVendorName { get; set; }
        public string? RMWHEREUSED { get; set; }
        public string? FGName { get; set; }
    }
}
