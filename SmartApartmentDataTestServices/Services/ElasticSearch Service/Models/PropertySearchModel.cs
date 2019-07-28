using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using SmartApartmentDataTestDataModule.DTOs;

namespace SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models
{
    [ElasticsearchType(Name = "property")]
    public class PropertySearchModel
    {
        #region Properties

        [JsonProperty("propertyID")]
        public long Id { get; set; }

        public string Name { get; set; }

        public string FormerName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string Market { get; set; }

        public string State { get; set; }

        public string FullAddress { get; set; }

        public float Lat { get; set; }

        public float Lng { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up address property for indexing
        /// </summary>
        public void SetupAddress()
        {
            var splitTest = FullAddress.Split(' ');

            if (int.TryParse(splitTest[0], out int b))
            {
                var temp = splitTest[0];
                for (var i = 0; i < splitTest.Length - 1; i++)
                {
                    splitTest[i] = splitTest[i + 1];
                }
                splitTest[splitTest.Length - 1] = temp;

                FullAddress = string.Join(" ", splitTest);
            }
        }

        /// <summary>
        /// Creates PropertySearchModel from a dto object
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static PropertySearchModel FromDto(PropertyDto dto) => dto != null
            ? new PropertySearchModel
            {
                Id = dto.PropertyID,
                Name = dto.Name,
                FormerName = dto.FormerName,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                Market = dto.Market,
                State = dto.State,
                Lat = dto.Lat,
                Lng = dto.Lng
            } : null;

        #endregion
    }
}
