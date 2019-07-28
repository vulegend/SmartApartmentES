using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class UserModel
    {
        #region Properties

        public int Id { get; set; }
        public string Auth0Subject { get; set; }

        #endregion

        #region Public Methods

        public static UserModel FromDbObject(Entity.user dbObject) => dbObject != null
            ? new UserModel
            {
                Id = dbObject.pk_id,
                Auth0Subject = dbObject.auth0_subject
            }
            : null;

        #endregion
    }
}
