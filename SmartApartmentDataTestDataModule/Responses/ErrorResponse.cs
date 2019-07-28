using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Responses
{
    public class ErrorResponse
    {
        #region Properties

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructor

        public ErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        #endregion
    }
}
