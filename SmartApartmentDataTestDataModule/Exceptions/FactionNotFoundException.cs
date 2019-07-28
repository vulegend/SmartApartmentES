using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class FactionNotFoundException : Exception
    {
        public const int ERROR_CODE = 13;

        public FactionNotFoundException(string message) : base(message)
        {
        }

        public static FactionNotFoundException FromId(int id) =>
            new FactionNotFoundException($"Faction with the id {id} was not found in the database.");

        public static FactionNotFoundException FromName(string name) =>
            new FactionNotFoundException($"Faction with the name {name} was not found in the database");
    }
}
