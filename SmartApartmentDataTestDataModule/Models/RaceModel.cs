using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class RaceModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPlayable { get; set; }

        #endregion

        #region Public Methods

        public static RaceModel FromDbObject(Entity.race dbObject) => dbObject != null
            ? new RaceModel
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
