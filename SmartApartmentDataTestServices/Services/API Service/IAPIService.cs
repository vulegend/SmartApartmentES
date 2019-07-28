using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestServices.Services.API_Service
{
    public interface IAPIService
    {
        HttpClient GetClient();
        Task<APIResponse> SendRequestAsync(APIRequest request);
    }
}
