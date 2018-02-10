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
        /// <param name="config"><see cref="HttpConfiguration"/></param>
        public static void Register<TU, T>(HttpConfiguration config)
            where T : TU
        {
            Container.RegisterType<TU, T>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(Container);
        }

        /// <summary>
        /// Register type into Container for Dependency injection
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"><see cref="HttpConfiguration"/></param>
        /// <param name="paramaters">additional contructor parameter</param>
        public static void Register<TU, T>(HttpConfiguration config, params object[] paramaters)
            where T : TU
        {
            Container.RegisterType<TU, T>(new HierarchicalLifetimeManager(), new InjectionConstructor(paramaters));
            config.DependencyResolver = new UnityResolver(Container);
        }
    }
}
