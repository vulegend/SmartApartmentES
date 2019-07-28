using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.ServiceLocation;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestDataModule.Models;
using WebGrease.Css.Extensions;

namespace SmartApartmentDataTest.Auth
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        #region Private Fields and Properties

        //The permission required to authorize
        private readonly string _permission;

        //Can resolve from the action context as well
        private ISmartApartmentDataTestDataAccess _dataAccess =>
            ServiceLocator.Current.GetInstance<ISmartApartmentDataTestDataAccess>();

        #endregion

        #region Constructor

        public PermissionAuthorizeAttribute(string permission)
        {
            _permission = permission;
        }

        #endregion

        #region Public Methods

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            try
            {
                var domain = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";

                ClaimsPrincipal principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

                var permissionClaims = principal?.Claims.Where(c => c.Type == "permissions" && c.Issuer == domain);

                //Check if the claim is present and if the required permission is in the list of the users permissions
                if (permissionClaims != null && permissionClaims.Select(x => x.Value).Any(p => p == _permission))
                {
                    //If authorized we save the user model in the request scope to be re-used in controller
                    PopulateActionContext(actionContext, principal?.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" && x.Issuer == domain)?.Value);
                    return;
                }

                HandleUnauthorizedRequest(actionContext);
            }
            catch (Exception e)
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds UserModel to the current request scope
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="auth0Subject"></param>
        private void PopulateActionContext(HttpActionContext actionContext, string auth0Subject)
        {
            var userModel = _dataAccess.GetUserByAuth0Subject(auth0Subject);

            if (userModel != null)
            {
                actionContext.Request.Properties.Add("user", userModel);
                return;
            }

            _dataAccess.RegisterUser(auth0Subject);
            actionContext.Request.Properties.Add("user", new UserModel
            {
                Auth0Subject = auth0Subject
            });
        }

        #endregion

    }
}