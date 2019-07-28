using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using SmartApartmentDataTestDataModule.DTOs;

namespace SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models
{
    [ElasticsearchType(Name = "management")]
    public class ManagementSearchModel
    {
        #region Properties

        [JsonProperty("mgmtID")]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Market { get; set; }

        public string State { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates ManagementSearchModel from a dto object
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static ManagementSearchModel FromDto(ManagementDto dto) => dto != null
            ? new ManagementSearchModel
            {
                Id = dto.MgmtID,
                Name = dto.Name,
                Market = dto.Market,
                State = dto.State
            }
            : null;

        #endregion
    }
}
