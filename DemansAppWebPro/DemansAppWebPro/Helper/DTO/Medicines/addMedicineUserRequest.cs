namespace DemansAppWebPro.Helper.DTO.Medicines
{
    public class addMedicineUserRequest
    {
        public int MedicineId { get; set; }
        public int UserId { get; set; }

        public bool Moon { get; set; }
        public bool Afternoon { get; set; }
        public bool Evening { get; set; }
        public bool Night { get; set; }
        public string MoonTime { get; set; }
        public string AfternoonTime { get;set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       
    }
}
