using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models;

namespace SmartApartmentDataTestServices.Services.ElasticSearch_Service
{
    public interface IElasticSearchService
    {
        #region Index Management

        void Setup();
        string IndexProperty(PropertyDto propertyDto);
        string IndexManagement(ManagementDto managementDto);
        void DeleteIndex(string indexName);

        #endregion

        #region Search

        SearchResponse GlobalSearch(string phrase, int size = 25);
        SearchResponse MarketSearch(string phrase, string market, int size = 25);

        #endregion
    }
}
