using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class FactionModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPlayable { get; set; }

        #endregion

        #region Public Methods

        public static FactionModel FromDbObject(Entity.faction dbObject) => dbObject != null
            ? new FactionModel
            {
                Id = dbObject.pk_id,
                Name = dbObject.name,
                Description = dbObject.description,
                IsPlayable = dbObject.playable
            }
            : null;

        #endregion
    }
}
