using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class GuildModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Realm { get; set; }
        public string Region { get; set; }
        public FactionModel Faction { get; set; }

        #endregion

        #region Public Methods

        public static GuildModel FromDbObject(Entity.guild dbObject) => dbObject != null
            ? new GuildModel
            {
                Id = dbObject.pk_id,
                Name = dbObject.name,
                Realm = dbObject.realm,
                Region = dbObject.region,
                Faction = FactionModel.FromDbObject(dbObject.faction)
            } : null;

        #endregion
    }
}
