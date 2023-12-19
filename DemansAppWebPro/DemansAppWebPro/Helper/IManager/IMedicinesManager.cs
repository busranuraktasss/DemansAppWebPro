using DemansAppWebPro.Helper.Client;
using DemansAppWebPro.Helper.DTO.Medicines;

namespace DemansAppWebPro.Helper.IManager
{
    public interface IMedicinesManager
    {
        Task<ClientResult<MedicineRequest>> getAllMedicines();
    }
}
