using SmartApartmentDataTest.Auth;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestServices.Services.ElasticSearch_Service;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SmartApartmentDataTest.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SmartApartmentDataTest.App_Start.NinjectWebCommon), "Stop")]

namespace SmartApartmentDataTest.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Http;
    using CommonServiceLocator.NinjectAdapter.Unofficial;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using SmartApartmentDataTestServices.Services.API_Service;

    public static class NinjectWebCommon 
    {
        #region Private Fields

        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(bootstrapper.Kernel));
            GlobalConfiguration.Configuration.DependencyResolver = new LocalNinjectDependencyResolver(bootstrapper.Kernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISmartApartmentDataTestDataAccess>().To<SmartApartmentDataTestDataAccess>().InRequestScope();
            kernel.Bind<IAPIService>().To<APIService>().InRequestScope();
            kernel.Bind<IElasticSearchService>().To<ElasticSearchService>().InRequestScope();
        }

        internal class LocalNinjectDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
        {
            private readonly IKernel _kernel;

            public LocalNinjectDependencyResolver(IKernel kernel)
            {
                _kernel = kernel;
            }

            public System.Web.Http.Dependencies.IDependencyScope BeginScope()
            {
                return this;
            }

            public object GetService(Type serviceType)
            {
                return _kernel.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                try
                {
                    return _kernel.GetAll(serviceType);
                }
                catch (Exception)
                {
                    return new List<object>();
                }
            }

            public void Dispose()
            {
            }
        }
    }


}