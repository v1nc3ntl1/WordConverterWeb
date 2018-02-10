
using System.Configuration;
using Kernel.Abstraction;

namespace Kernel
{
    public class AppSettings : ISettings<string>
    {
        public string Get(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
