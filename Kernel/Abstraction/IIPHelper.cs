
namespace Kernel.Abstraction
{
    /// <summary>
    /// Ip address helper interface
    /// </summary>
    public interface IIpHelper
    {
        /// <summary>
        /// Get the ip address given a request
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GetIp(RequestPipelineData data);
    }
}