using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SmartApartmentDataTest.Filters
{
    public class ModelStateFilterAttribute : ActionFilterAttribute
    {
        #region Private Fields

        private static readonly ConcurrentDictionary<HttpActionDescriptor, IList<string>> RequiredParams =
            new ConcurrentDictionary<HttpActionDescriptor, IList<string>>();

        #endregion

        #region Public Methods

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requiredParams = GetRequiredParams(actionContext);
            foreach (var requiredParam in requiredParams)
            {
                if (!actionContext.ActionArguments.TryGetValue(requiredParam, out var value) || value == null)
                    actionContext.ModelState.AddModelError(requiredParam, "Parameter -- " + requiredParam + " -- was not specified.");
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets all the parameters that are required in a request
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetRequiredParams(HttpActionContext actionContext)
        {
            var result = RequiredParams.GetOrAdd(actionContext.ActionDescriptor,
                descriptor => descriptor.GetParameters()
                    .Where(p => !p.IsOptional && p.DefaultValue == null &&
                                !p.ParameterType.IsValueType &&
                                p.ParameterType != typeof(string))
                    .Select(p => p.ParameterName)
                    .ToList());

            return result;
        }

        #endregion
    }
}