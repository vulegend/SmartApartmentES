using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestServices.Const;

namespace SmartApartmentDataTestServices.Services.API_Service
{
    public class APIRequest
    {
        #region Properties

        public RequestMethod Method { get; private set; }
        public string Endpoint { get; private set; }
        public Dictionary<string, string> RequestParameters { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }

        #endregion

        #region Constructor

        public APIRequest(RequestMethod method, string endpoint)
        {
            Method = method;
            Endpoint = endpoint;
        }

        #endregion
    }
}
