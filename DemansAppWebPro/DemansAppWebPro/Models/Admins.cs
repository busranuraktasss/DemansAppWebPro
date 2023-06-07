using System.ComponentModel.DataAnnotations;

namespace DemansAppWebPro.Models
{
    public class Admins
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
