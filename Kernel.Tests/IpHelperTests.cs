
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace Kernel.Tests
{
    public class IpHelperTests
    {
        private readonly IpHelper _ipHelper;

        public IpHelperTests()
        {
            _ipHelper = new IpHelperStub();
        }

        [Test]
        public void GetIpTest_WithMS_HttpContextProperties_WillReturnGetHostIp()
        {
            // Arrange
            var data = new RequestPipelineData()
            {
                RequestMessage = new HttpRequestMessage()
                {
                    Properties = { { new KeyValuePair<string, object>("MS_HttpContext", "value") } }
                }
            };
            ((IpHelperStub)_ipHelper).Ip = "127.0.0.1";

            // Act
            var actual = _ipHelper.GetIp(data);

            // Assert
            Assert.AreEqual("127.0.0.1", actual);
        }

        [Test]
        public void GetIpTest_WithoutMS_HttpContextProperties_WillReturnNull()
        {
            // Arrange
            var data = new RequestPipelineData()
            {
                RequestMessage = new HttpRequestMessage()
                {
                    Properties = { { new KeyValuePair<string, object>("xxx", "value") } }
                }
            };

            // Act
            var actual = _ipHelper.GetIp(data);

            // Assert
            Assert.IsNull(actual);
        }

        public class IpHelperStub : IpHelper
        {
            public string Ip { get; set; }

            protected override string GetHostIp()
            {
                return this.Ip;
            }
        }
    }
}
