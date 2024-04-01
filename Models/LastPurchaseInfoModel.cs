using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
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
