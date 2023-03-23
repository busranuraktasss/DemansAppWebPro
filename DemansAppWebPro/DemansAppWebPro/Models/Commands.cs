using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class Commands
    {
        [Key]
        public int Id { get; set; }

        public string ProcessName { get; set; }

        public bool? Status { get; set; }
 
        public int? UserId { get; set; }

        public int? CompanionId { get; set; }

    }
}
