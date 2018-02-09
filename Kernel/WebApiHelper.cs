using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Kernel
{
    /// <summary>
    /// Web Api Helper class
    /// </summary>
    public static class WebApiHelper
    {
        private static readonly UnityContainer Container = new UnityContainer();

        /// <summary>
        /// Register type into Container for Dependency injection
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        public static void Register<TU, T>(HttpConfiguration config)
            where T : TU
        {
            Container.RegisterType<TU, T>(new HierarchicalLifetimeManager(), new InjectionConstructor(true, true, true));
            config.DependencyResolver = new UnityResolver(Container);
        }
    }
}
