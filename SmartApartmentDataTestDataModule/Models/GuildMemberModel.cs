using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class GuildMemberModel
    {
        #region Properties

        public int Id { get; set; }
        public PlayerModel Player { get; set; }
        public GuildModel Guild { get; set; }

        #endregion

        #region Public Methods

        public static GuildMemberModel FromDbObject(Entity.guild_members dbObject) => dbObject != null
            ? new GuildMemberModel
            {
                Id = dbObject.pk_id,
                Guild = GuildModel.FromDbObject(dbObject.guild),
                Player = PlayerModel.FromDbObject(dbObject.player)
            }
            : null;

        #endregion
    }
}
