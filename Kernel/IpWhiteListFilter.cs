using System;
using System.Linq;
using System.Net;
using Abstraction.Kernel;
using Kernel.Abstraction;

namespace Kernel
{
    /// <summary>
    /// Request filter that control whether to Allow the request if client ip address matches the ip lists in the WhiteListedIPAddresses settings.
    /// If no WhiteListedIPAddresses is defined, it will Allow the request.
    /// If additional <see cref="IRequestFilter"/> is pass through constructor, it will also run IsAllow method of other implementation of <see cref="IRequestFilter"/>
    /// </summary>
    public class IpWhiteListFilter : IRequestFilter
    {
        private readonly ISettings<string> _settings;
        private readonly IRequestFilter _additionalRequestFilter;
        private readonly IIpHelper _ipHelper;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="settings">Interface to get a setting</param>
        /// <param name="ipHelper"><see cref="IIpHelper"/> interface to get the ip address of the client</param>
        public IpWhiteListFilter(ISettings<string> settings, IIpHelper ipHelper) : this(settings, ipHelper, null) { }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="settings">Interface to get a setting</param>
        /// <param name="ipHelper"><see cref="IIpHelper"/> interface to get the ip address of the client</param>
        /// <param name="requestFilter">additional request filter to </param>
        public IpWhiteListFilter(ISettings<string> settings, IIpHelper ipHelper, IRequestFilter requestFilter)
        {
            _settings = settings;
            _ipHelper = ipHelper;
            _additionalRequestFilter = requestFilter;
        }

        /// <summary>
        /// Determine whether to Allow the Request 
        /// </summary>
        /// <param name="data"><see cref="RequestPipelineData"/> containing some information about the current request.</param>
        /// <returns>Flag indicate whether to Allow the Request</returns>
        public bool IsAllow(RequestPipelineData data)
        {
            bool allow = IsClientIpAllow(data);
            if (!allow) return false;

            if (_additionalRequestFilter != null)
            {
                return _additionalRequestFilter.IsAllow(data);
            }

            return true;
        }

        /// <summary>
        /// Determine whether to Allow the Request by matching the client ip against the lists defined in WhiteListedIPAddresses settings
        /// </summary>
        /// <param name="data"><see cref="RequestPipelineData"/> containing some information about the current request.</param>
        /// <returns>Flag indicate whether to Allow the Request</returns>
        protected virtual bool IsClientIpAllow(RequestPipelineData data)
        {
            var whiteListedIPs = _settings.Get(WellknownConstants.WhiteListIp);

            if (string.IsNullOrEmpty(whiteListedIPs)) return true;

            var whiteListIpList = whiteListedIPs.Split(',').ToList();
            var ipAddressString = _ipHelper.GetIp(data);
            var ipAddress = IPAddress.Parse(ipAddressString);
            return whiteListIpList.Any(a => a.Trim()
                .Equals(ipAddressString, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
