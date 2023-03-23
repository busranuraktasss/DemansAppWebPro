using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class LocationInformation
    {
        [Key]
        public int Id { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public int? UserId { get; set; }
    }
}
