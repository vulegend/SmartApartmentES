using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class ClassNotFoundException : Exception
    {
        public const int ERROR_CODE = 12;

        public ClassNotFoundException(string message) : base(message)
        {
        }

        public static ClassNotFoundException FromId(int id) => 
            new ClassNotFoundException($"Class with the id {id} was not found in the database.");

        public static ClassNotFoundException FromName(string name) =>
            new ClassNotFoundException($"Class with the name {name} was not found in the database");
    }
}
