using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Entity
{
    public partial class SmartApartmentEntities
    {
        public SmartApartmentEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
