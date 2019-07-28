using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class PlayerAlreadyExistsException : Exception
    {
        public const int ERROR_CODE = 16;

        public PlayerAlreadyExistsException() : base("Player with that name already exists.")
        {
        }
    }
}
