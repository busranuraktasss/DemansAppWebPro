using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DemansAppWebPro.Models
{
    public class AudioBooks
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public int? UserId { get; set; }
    }
}
