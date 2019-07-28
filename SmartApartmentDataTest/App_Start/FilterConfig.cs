using SmartApartmentDataTest.Filters;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SmartApartmentDataTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpConfiguration config)
        {
            //Register the model state validator for every request
            config.Filters.Add(new ModelStateFilterAttribute());
        }
    }
}
