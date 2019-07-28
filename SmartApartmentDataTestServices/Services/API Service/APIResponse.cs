using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestServices.Services.API_Service
{
    public class APIResponse
    {
        #region Properties

        public HttpStatusCode StatusCode { get; private set; }
        public string Response { get; private set; }
        public bool Success => StatusCode == HttpStatusCode.OK;

        #endregion

        #region Constructor

        public APIResponse(HttpStatusCode statusCode, string response)
        {
            StatusCode = statusCode;
            Response = response;
        }

        #endregion
    }
}
