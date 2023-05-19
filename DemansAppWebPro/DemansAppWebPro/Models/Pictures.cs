using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class Pictures
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public int? UserId { get; set; }
        public Boolean Status { get; set; }
    }
}
