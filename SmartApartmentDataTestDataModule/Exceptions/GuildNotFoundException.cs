using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class GuildNotFoundException : Exception
    {
        public const int ERROR_CODE = 25;
        public GuildNotFoundException(string message) : base(message)
        {

        }

        public static GuildNotFoundException FromName(string guildName) =>
            new GuildNotFoundException($"Guild with name: {guildName} does not exist in the database");
    }
}
