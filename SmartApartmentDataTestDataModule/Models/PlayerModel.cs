using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class PlayerModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Region { get; set; }
        public ClassModel Class { get; set; }
        public FactionModel Faction { get; set; }
        public RaceModel Race { get; set; }

        #endregion

        #region Public Methods


        public static PlayerModel FromDbObject(Entity.player dbObject) => dbObject != null
            ? new PlayerModel
            {
                Id = dbObject.pk_id,
                Name = dbObject.name,
                Realm = dbObject.realm,
                Region = dbObject.region,
                Class = ClassModel.FromDbObject(dbObject.@class),
                Faction = FactionModel.FromDbObject(dbObject.faction),
                Race = RaceModel.FromDbObject(dbObject.race)
            }
            : null;

        #endregion
    }
}
