namespace DemansAppWebPro.Helper.DTO.Users
{
    public class showUsersRequest
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string EmergencyPhone { get; set; }

        public string UserName { get; set; }

        public string Surname { get; set; }

        public string Phone { get; set; }

        public bool? Sex { get; set; }
    }
}
