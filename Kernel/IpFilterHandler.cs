using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Kernel;

namespace Kernel
{
    public class IpFilterHandler : DelegatingHandler
    {
        private readonly IRequestFilter _requestFilter;

        public IpFilterHandler(IRequestFilter requestFilter)
        {
            _requestFilter = requestFilter;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.AllowIp(request))
            {
                return await SendBaseAysnc(request, cancellationToken);
            }

            return request
                .CreateErrorResponse(HttpStatusCode.Unauthorized
                    , "Not authorized to view/access this resource");
        }

        protected virtual bool AllowIp(HttpRequestMessage request)
        {
            return _requestFilter.IsAllow(new RequestPipelineData() {RequestMessage = request});
        }

        protected virtual Task<HttpResponseMessage> SendBaseAysnc(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}
