using System.Web;
using Kernel.Abstraction;

namespace Kernel
{
    /// <summary>
    /// Help class to get ip address of client based on request.
    /// </summary>
    public class IpHelper : IIpHelper
    {
        /// <summary>
        /// Get Client ip address
        /// </summary>
        /// <param name="data"><see cref="RequestPipelineData"/>wrapper of request data</param>
        /// <returns>ip address of client</returns>
        public string GetIp(RequestPipelineData data)
        {
            // Web Hosting
            if (data.RequestMessage.Properties.ContainsKey("MS_HttpContext"))
            {
                return GetHostIp();
            }

            return null;
        }

        /// <summary>
        /// Get Client ip address from User Host Address
        /// </summary>
        /// <returns>ip address of client</returns>
        protected virtual string GetHostIp()
        {
            return HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;
        }
    }
}
