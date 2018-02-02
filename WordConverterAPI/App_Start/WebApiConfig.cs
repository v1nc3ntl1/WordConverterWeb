using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Kernel;
using WordConverterLibrary;

namespace WordConverterAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            WebApiHelper.Register<IWordConverterProvider, WordConverterProvider>(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
