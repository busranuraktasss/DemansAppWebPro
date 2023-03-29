namespace DemansAppWebPro.Helper.DTO.Companions
{
    public class addCompanionsRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Surname { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; } 
        public bool? Sex { get; set; }
    }
}
