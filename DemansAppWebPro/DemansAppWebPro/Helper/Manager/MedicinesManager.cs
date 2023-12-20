using DemansAppWebPro.Helper.Client;
using DemansAppWebPro.Helper.IManager;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using DemansAppWebPro.Helper.DTO.Medicines;
using System.Net.Http.Headers;
using MySqlX.XDevAPI;

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
                var res = await http.GetAsync(uri);
                var EmpResponse = await res.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<ClientResult<MedicineRequest>>(EmpResponse);
                return response;

            }
        }
       

    }
}
