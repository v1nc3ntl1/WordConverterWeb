using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Kernel;
using NUnit.Framework;

namespace Kernel.Tests
{
    public class IpFilterHandlerTests
    {
        private IpFilterHandlerStub _handler;

        public IpFilterHandlerTests()
        {
            _handler = new IpFilterHandlerStub(null);
        }

        [Test]
        public void SendAsyncTest_WithAllowIpIsTrue_WillCallBaseSendAsync()
        {
            // Arrange
            var called = false;
            var expected = new HttpResponseMessage(HttpStatusCode.OK);
            _handler.AllowIpProp = true;
            _handler.SendAsyncFunc = (message, token) =>
            {
                called = true;
                return Task.FromResult<HttpResponseMessage>(expected);
            };

            // Act
            var actual = _handler.SendAsyncWrapper(new HttpRequestMessage(HttpMethod.Get, "http://localhost:4200"), CancellationToken.None);

            // Assert
            Assert.IsTrue(called);
            Assert.AreEqual(expected, actual.Result);
            ;
        }

        [Test]
        public void SendAsyncTest_WithAllowIpIsFalse_WillReturnUnauthorized()
        {
            // Arrange
            _handler.AllowIpProp = false;

            // Act
            var actual = _handler.SendAsyncWrapper(new HttpRequestMessage(HttpMethod.Get, "http://localhost:4200"), CancellationToken.None);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, actual.Result.StatusCode);
        }

        internal class IpFilterHandlerStub : IpFilterHandler
        {
            internal Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> SendAsyncFunc { get; set; }
            internal bool? AllowIpProp { get; set; }

            protected override Task<HttpResponseMessage> SendBaseAysnc(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (SendAsyncFunc != null)
                {
                    return SendAsyncFunc(request, cancellationToken);
                }

                return base.SendAsync(request, cancellationToken);
            }

            internal async Task<HttpResponseMessage> SendAsyncWrapper(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            protected override bool AllowIp(HttpRequestMessage request)
            {
                return AllowIpProp ?? base.AllowIp(request);
            }

            public IpFilterHandlerStub(IRequestFilter requestFilter) : base(requestFilter)
            {
            }
        }
    }
}
