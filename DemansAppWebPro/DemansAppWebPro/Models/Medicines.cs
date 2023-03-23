using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class Medicines
    {
        [Key]
        public int Id { get; set; }

        public string Day { get; set; }

        public string Name { get; set; }    

        public DateTime? Time { get; set; }

        public string UsageDuration { get; set; }

        public string UsagePurpose { get; set; }

        public int? UserId { get; set; }

        public int? CompanionId { get; set; }
    }
}
