using DemansAppWebPro.Helper.Client;
using DemansAppWebPro.Helper.IManager;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using DemansAppWebPro.Helper.DTO.Medicines;
using System.Net.Http.Headers;

namespace DemansAppWebPro.Helper.Manager
{
    public class MedicinesManager :IMedicinesManager
    {
        private readonly IConfiguration _configuration;

        public MedicinesManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ClientResult<MedicineRequest>> getAllMedicines()
        {
            using (var http = new HttpClient())
            { 
                var baseUrl = _configuration.GetValue<string>("BaseUrl");
                var uri = $"{baseUrl}api/Medicines/getAllMedicines";
                var content = new StringContent(JsonConvert.SerializeObject(""));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                http.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                var res = await http.PostAsync(uri, content);
                var EmpResponse = await res.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ClientResult<MedicineRequest>>(EmpResponse);
                return response;
            }
        }
       

    }
}
