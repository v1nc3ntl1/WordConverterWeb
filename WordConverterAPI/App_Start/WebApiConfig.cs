using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Kernel;
using Newtonsoft.Json.Serialization;
using WordConverterLibrary;

namespace WordConverterAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            WebApiHelper.Register<IWordConverterProvider, SimpleWordConverterProvider>(config);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, Action = "Get" }
            );
        }
    }
}
