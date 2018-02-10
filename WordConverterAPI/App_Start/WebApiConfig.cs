using System.Web.Http;
using Abstraction.Kernel;
using Kernel;
using Kernel.Abstraction;
using Newtonsoft.Json.Serialization;
using WordConverterLibrary;

namespace WordConverterAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            WebApiHelper.Register<IWordConverterProvider, SimpleWordConverterProvider>(config, true, true, true);
            WebApiHelper.Register<ISettings<string>, AppSettings>(config);
            WebApiHelper.Register<IIpHelper, IpHelper>(config);
            WebApiHelper.Register<IRequestFilter, IpWhiteListFilter>(config);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, Action = "Get" }
            );
            config.MessageHandlers.Add(new IpFilterHandler(new IpWhiteListFilter(new AppSettings(), new IpHelper(), null)));
        }
    }
}
