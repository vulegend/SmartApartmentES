using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using SmartApartmentDataTest.Requests;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Responses;
using SmartApartmentDataTestServices.Const;
using SmartApartmentDataTestServices.Services.API_Service;
using SmartApartmentDataTestUtility.Logger;

namespace SmartApartmentDataTest.Controllers
{
    public class AccountController : ApiController
    {
        #region Private Fields

        private readonly IAPIService _apiService;

        #endregion

        #region Constructor

        public AccountController(ISmartApartmentDataTestDataAccess dataAccess, IAPIService apiService)
        {
            _apiService = apiService;
        }

        #endregion

        #region POST

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/authenticate")]
        public async Task<HttpResponseMessage> Login(LoginDto request)
        {
            try
            {
                var auth0Domain = ConfigurationManager.AppSettings["Auth0Domain"];
                var auth0ClientId = ConfigurationManager.AppSettings["Auth0ClientId"];
                var auth0ClientSecret = ConfigurationManager.AppSettings["Auth0ClientSecret"];

                AuthenticationApiClient client =
                    new AuthenticationApiClient(
                        new Uri($"https://{auth0Domain}/"));

                //Get the auth token from auth0 login endpoint
                var result = await client.GetTokenAsync(new ResourceOwnerTokenRequest
                {
                    ClientId = auth0ClientId,
                    ClientSecret = auth0ClientSecret,
                    Scope = "openid",
                    Audience = "http://smartdatatest.com/",
                    Realm = "Username-Password-Authentication", // Specify the correct name of your DB connection
                    Username = request.Username,
                    Password = request.Password
                });

                return Request.CreateResponse(HttpStatusCode.OK, result.AccessToken);
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message, "ErrorCode:222");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(222, e.Message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/resetPassword")]
        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var apiRequest = new APIRequest(RequestMethod.POST,
                    $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/dbconnections/change_password")
                {
                    RequestHeaders = new Dictionary<string, string>(),
                    RequestParameters = new Dictionary<string, string>()
                    {
                        {"client_id", ConfigurationManager.AppSettings["Auth0ClientId"]},
                        {"email", request.Username},
                        {"connection", "Username-Password-Authentication"}
                    }
                };

                var apiResponse = await _apiService.SendRequestAsync(apiRequest);

                if (apiResponse.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse.Response);
                }

                return Request.CreateResponse(apiResponse.StatusCode, apiResponse.Response);
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/register")]
        public async Task<HttpResponseMessage> RegisterUser(LoginDto request)
        {
            try
            {
                var apiRequest = new APIRequest(RequestMethod.POST,
                    $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/dbconnections/signup")
                {
                    RequestHeaders = new Dictionary<string, string>(),
                    RequestParameters = new Dictionary<string, string>()
                    {
                        {"client_id", ConfigurationManager.AppSettings["Auth0ClientId"]},
                        {"email", request.Username},
                        {"password", request.Password },
                        {"connection", "Username-Password-Authentication"}
                    }
                };

                var apiResponse = await _apiService.SendRequestAsync(apiRequest);

                if (apiResponse.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse.Response);
                }

                return Request.CreateResponse(apiResponse.StatusCode, apiResponse.Response);
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
