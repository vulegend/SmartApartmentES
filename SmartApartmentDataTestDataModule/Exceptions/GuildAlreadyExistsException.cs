using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class GuildAlreadyExistsException : Exception
    {
        public const int ERROR_CODE = 17;

        public GuildAlreadyExistsException() : base("Guild with that name already exists.")
        {
        }
    }
}
