using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        public const int ERROR_CODE = 20;
        public PlayerNotFoundException(string message) : base(message)
        {

        }

        public static PlayerNotFoundException FromName(string name) => new PlayerNotFoundException($"Player with name: {name} does not exist in the database");

        public static PlayerNotFoundException FromUserId(int userId) => new PlayerNotFoundException($"User with ID: {userId} does not have a character");
    }
}
