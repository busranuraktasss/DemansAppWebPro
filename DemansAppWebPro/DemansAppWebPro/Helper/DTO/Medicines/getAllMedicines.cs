namespace DemansAppWebPro.Helper.DTO.Medicines
{
    public class getAllMedicines
    {
        public int id { get; set; }
        public string name { get; set; }
        public string usageDuration { get; set; }
        public bool status { get; set; }
        public object usagePurpose { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool moon { get; set; }
        public bool afternoon { get; set; }
        public bool evening { get; set; }
        public bool night { get; set; }
        public object moonTime { get; set; }
        public object afternoonTime { get; set; }
        public object eveningTime { get; set; }
        public object nightTime { get; set; }
        public object userId { get; set; }
    }
}
