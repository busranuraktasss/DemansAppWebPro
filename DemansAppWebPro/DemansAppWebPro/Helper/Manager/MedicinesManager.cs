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

        public async Task<ClientResult<getAllMedicines>> getAllMedicines()
        {
            using (var http = new HttpClient())
            {

                try
                {
                    var baseUrl = _configuration.GetValue<string>("BaseUrl");
                    var uri = $"{baseUrl}api/Medicines/getAllMedicines";
                    var res = await http.GetAsync(uri);
                    var EmpResponse = await res.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<getAllMedicines>(EmpResponse);
                    var _res = new ClientResult<getAllMedicines>() {  Data = response, Code = 200, Message = "Success" };
                    return _res;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
        }
       

    }
}
