using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class QoutationModel
    {
        [Key]
        public int Id { get; set; }

        public string PartNumber { get; set; }
    }
}
