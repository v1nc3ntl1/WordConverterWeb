using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Kernel
{
    public static class WebApiHelper
    {
        private static readonly UnityContainer Container = new UnityContainer();

        public static void Register<TU, T>(HttpConfiguration config)
            where T : TU
        {
            Container.RegisterType<TU, T>(new HierarchicalLifetimeManager(), new InjectionConstructor(true, true, true));
            config.DependencyResolver = new UnityResolver(Container);
        }
    }
}
