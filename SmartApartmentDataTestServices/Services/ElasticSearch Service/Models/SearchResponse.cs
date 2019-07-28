using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models
{
    #region Search Response Classes

    public class SearchResponse
    {
        public List<BaseSearchResponse> SearchResponses { get; set; }
    }

    public class BaseSearchResponse
    {
        public double Score { get; set; }
    }

    public class PropertySearchResponse : BaseSearchResponse
    {
        public PropertySearchModel PropertyModel { get; set; }
    }

    public class ManagementSearchResponse : BaseSearchResponse
    {
        public ManagementSearchModel ManagementModel { get; set; }
    }

    #endregion
}
