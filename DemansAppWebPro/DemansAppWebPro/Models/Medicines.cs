using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class Medicines
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }  
        public string UsageDuration { get; set; }
        public bool? Status { get; set; }
        public string UsagePurpose { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Moon { get; set; }
        public bool Afternoon { get; set; }
        public bool Evening { get; set; }
        public bool Night { get; set; }
        public string MoonTime { get; set; }
        public string AfternoonTime { get; set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set; } 
        public int? UserId { get; set; }
    }
}
