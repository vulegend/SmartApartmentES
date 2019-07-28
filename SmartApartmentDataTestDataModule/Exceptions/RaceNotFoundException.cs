using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class RaceNotFoundException : Exception
    {
        public const int ERROR_CODE = 14;

        public RaceNotFoundException(string message) : base(message)
        {
        }

        public static RaceNotFoundException FromId(int id) =>
            new RaceNotFoundException($"Race with the id {id} was not found in the database.");

        public static RaceNotFoundException FromName(string name) =>
            new RaceNotFoundException($"Race with the name {name} was not found in the database");
    }
}
