using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartApartmentDataTest.Auth;
using SmartApartmentDataTest.Requests;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Responses;
using SmartApartmentDataTestServices.Services.ElasticSearch_Service;
using SmartApartmentDataTestServices.Services.ElasticSearch_Service.Models;
using SmartApartmentDataTestUtility.Logger;

namespace SmartApartmentDataTest.Controllers
{
    public class SearchController : ApiController
    {
        #region Private Fields

        private readonly IElasticSearchService _elasticSearchService;

        #endregion

        #region Constructor

        public SearchController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }

        #endregion

        #region POST

        [HttpPost]
        [Route("api/v1/search/setup")]
        [PermissionAuthorize("Admin")]
        public HttpResponseMessage SetupSearch()
        {
            try
            {
                _elasticSearchService.Setup();
                return Request.CreateResponse(HttpStatusCode.OK, "ElasticSearch is successfully setup");
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        [HttpPost]
        [Route("api/v1/search/indexProperty")]
        [PermissionAuthorize("Admin")]
        public HttpResponseMessage IndexProperty(PropertyDto request)
        {
            try
            {
                var response = _elasticSearchService.IndexProperty(request);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        [HttpPost]
        [Route("api/v1/search/indexManagement")]
        [PermissionAuthorize("Admin")]
        public HttpResponseMessage IndexManagement(ManagementDto request)
        {
            try
            {
                var response = _elasticSearchService.IndexManagement(request);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion

        #region DELETE

        [HttpDelete]
        [Route("api/v1/search/deleteIndex")]
        [PermissionAuthorize("Admin")]
        public HttpResponseMessage DeleteIndex(DeleteIndexRequest request)
        {
            try
            {
                _elasticSearchService.DeleteIndex(request.IndexName);
                return Request.CreateResponse(HttpStatusCode.OK, "Index successfully deleted");
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion

        #region GET

        [HttpGet]
        [Route("api/v1/search")]
        [PermissionAuthorize("User")]
        public HttpResponseMessage ExecuteSearch(SearchRequest request)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, request.Market == null ?
                    _elasticSearchService.GlobalSearch(request.Phrase, request.Size ?? 25) : _elasticSearchService.MarketSearch(request.Phrase, request.Market, request.Size ?? 25));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion
    }
}
