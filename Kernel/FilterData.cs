
using System.Net.Http;

namespace Kernel
{
    public class RequestPipelineData
    {
		/// <summary>
		/// Request Message
		/// </summary>
        public HttpRequestMessage RequestMessage { get; set; }
    }
}
