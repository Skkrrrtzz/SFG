using System.ComponentModel.DataAnnotations;

namespace ATSSFG.Models
{
    public class QuotationModel
    {
        [Key]
        public int Id { get; set; }

        public string PartNumber { get; set; }
    }
}