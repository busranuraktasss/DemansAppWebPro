namespace DemansAppWebPro.Helper.DTO.Medicines
{
    public class MedicineRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string UsagePurpose { get; set; }
        public string Moon { get; set; }
        public string Afternoon { get; set; }
        public string Evening { get; set; }
        public string Night { get; set; }
        public string MoonTime { get; set; }
        public string AfternoonTime { get; set; }
        public string EveningTime { get; set; }
        public string NightTime { get; set; }
        public string UsageDay { get; set; }
        public int UsageTime { get; set; }
        public string Time { get; set; }
        public int MedicineId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
