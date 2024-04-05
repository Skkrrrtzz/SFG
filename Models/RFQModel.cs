using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class RFQModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }
        public string Customer { get; set; }
        public string QuotationCode { get; set; }
        public int NoItems { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string CustomerPartNumber { get; set; }
        public string Rev { get; set; }
        public string Description { get; set; }
        public string OrigMPNRawMatFab { get; set; }
        public string OrigManufacturerFinishFabParts { get; set; }
        public string Commodity { get; set; }
        public int QtyperAssy { get; set; }
        public string BOMUOM { get; set; }
        public string PartIdentifier { get; set; }
    }
}
