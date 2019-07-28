using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.Entity;
using SmartApartmentDataTestDataModule.Models;
using SmartApartmentDataTestUtility.Encryption.AES;

namespace SmartApartmentDataTestDataModule.Data_Access
{
    public class SmartApartmentDataTestDataAccess : ISmartApartmentDataTestDataAccess
    {
        #region Private Fields

        private const string EncryptionKey = "8M5%'f+lta#..;JK}pX0J7^%A4bh${l'";
        private readonly Entity.SmartApartmentEntities _entities;

        #endregion

        #region Constructor

        public SmartApartmentDataTestDataAccess()
        {
            var encryptedConnectionString = ConfigurationManager.ConnectionStrings["SmartApartmentEntities"];
            var connectionString = AESEncryption.Decrypt(encryptedConnectionString.ConnectionString, EncryptionKey);
            _entities = new SmartApartmentEntities(connectionString);
        }


        #endregion

        #region User

        public void RegisterUser(string auth0Subject)
        {
            _entities.users.Add(new Entity.user
            {
                auth0_subject = auth0Subject
            });
            _entities.SaveChanges();
        }

        public UserModel GetUserByAuth0Subject(string auth0Subject)
        {
            return UserModel.FromDbObject(_entities.users.FirstOrDefault(x => x.auth0_subject.Equals(auth0Subject)));
        }

        #endregion
    }
}
