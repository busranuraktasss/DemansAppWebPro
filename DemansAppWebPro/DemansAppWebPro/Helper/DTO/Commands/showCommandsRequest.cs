namespace DemansAppWebPro.Helper.DTO.Commands

{
    public class showCommandsRequest
    {
        public int Id { get; set; }

        public string ProcessName { get; set; }

        public bool? Status { get; set; }
    }
}
