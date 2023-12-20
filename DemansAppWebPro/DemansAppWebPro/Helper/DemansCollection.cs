using DemansAppWebPro.Helper.IManager;
using DemansAppWebPro.Helper.Manager;

namespace DemansAppWebPro.Helper
{
    static class DemansCollection
    {
        public static IServiceCollection AddDemansCollection(this IServiceCollection services)
        {
            services.AddScoped<IMedicinesManager, MedicinesManager>();

            return services;
        }
    }
}
