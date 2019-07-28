using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Exceptions
{
    public class PlayerAlreadyInGuildException : Exception
    {
        public PlayerAlreadyInGuildException() : base("Player is already in a guild")
        {

        }
    }
}
