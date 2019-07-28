using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestServices.Const;

namespace SmartApartmentDataTestServices.Services.API_Service
{
    public class APIService : IAPIService
    {
        #region Public Methods

        public HttpClient GetClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpClientHandler handler = new HttpClientHandler
            {
                Proxy = null,
                UseProxy = false
            };
            return new HttpClient(handler);
        }

        public async Task<APIResponse> SendRequestAsync(APIRequest apiRequest)
        {
            var encodedContent = new FormUrlEncodedContent(apiRequest.RequestParameters);

            var client = GetClient();
            foreach (var pair in apiRequest.RequestHeaders)
                client.DefaultRequestHeaders.Add(pair.Key, pair.Value);

            HttpResponseMessage response = null;

            switch (apiRequest.Method)
            {
                case RequestMethod.POST:
                    response = await client.PostAsync(apiRequest.Endpoint, encodedContent).ConfigureAwait(false);
                    break;
                case RequestMethod.PUT:
                    response = await client.PutAsync(apiRequest.Endpoint, encodedContent).ConfigureAwait(false);
                    break;
                case RequestMethod.GET:
                    response = await client.GetAsync(apiRequest.Endpoint).ConfigureAwait(false);
                    break;
                case RequestMethod.DELETE:
                    response = await client.DeleteAsync(apiRequest.Endpoint).ConfigureAwait(false);
                    break;
                default:
                    await client.PostAsync(apiRequest.Endpoint, encodedContent).ConfigureAwait(false);
                    break;
            }

            if (response == null)
                return new APIResponse(HttpStatusCode.InternalServerError, "");

            var responseContent = await response.Content.ReadAsStringAsync();
            return new APIResponse(response.StatusCode, responseContent);
        }

        #endregion
    }
}
