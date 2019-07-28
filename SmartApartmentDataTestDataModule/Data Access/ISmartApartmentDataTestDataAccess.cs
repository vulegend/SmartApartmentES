using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.Models;

namespace SmartApartmentDataTestDataModule.Data_Access
{
    public interface ISmartApartmentDataTestDataAccess
    {
        #region User

        void RegisterUser(string auth0Subject);
        UserModel GetUserByAuth0Subject(string auth0Subject);

        #endregion
    }
}
