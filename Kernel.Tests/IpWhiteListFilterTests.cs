using Abstraction.Kernel;
using Kernel.Abstraction;
using NSubstitute;
using NUnit.Framework;

namespace Kernel.Tests
{
    public class IpWhiteListFilterTests
    {
        private IpWhiteListFilter _ipWhiteListFilter;

        [Test]
        public void IsAllowTest_WithNoWhiteListDefined_WillReturnTrue()
        {
            // Arrange
            var fakeSettings = Substitute.For<ISettings<string>>();
            fakeSettings.Get(WellknownConstants.WhiteListIp).Returns("");

            var fakeIpHelper = Substitute.For<IIpHelper>();
            fakeIpHelper.GetIp(null).ReturnsForAnyArgs("10.0.18.99");

            _ipWhiteListFilter = new IpWhiteListFilter(fakeSettings, fakeIpHelper, null);

            // Act
            var actual = _ipWhiteListFilter.IsAllow(new RequestPipelineData());

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsAllowTest_WithMatchClientIp_WillReturnTrue()
        {
            // Arrange
            var fakeSettings = Substitute.For<ISettings<string>>();
            fakeSettings.Get(WellknownConstants.WhiteListIp).Returns("10.0.18.99, 10.0.88.22");

            var fakeIpHelper = Substitute.For<IIpHelper>();
            fakeIpHelper.GetIp(null).ReturnsForAnyArgs("10.0.18.99");

            _ipWhiteListFilter = new IpWhiteListFilter(fakeSettings, fakeIpHelper, null);
            
            // Act
            var actual = _ipWhiteListFilter.IsAllow(new RequestPipelineData());

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsAllowTest_WithUnMatchClientIp_WillReturnFalse()
        {
            // Arrange
            var fakeSettings = Substitute.For<ISettings<string>>();
            fakeSettings.Get(WellknownConstants.WhiteListIp).Returns("10.0.18.99, 10.0.88.22");

            var fakeIpHelper = Substitute.For<IIpHelper>();
            fakeIpHelper.GetIp(null).ReturnsForAnyArgs("10.0.183.42");

            _ipWhiteListFilter = new IpWhiteListFilter(fakeSettings, fakeIpHelper, null);

            // Act
            var actual = _ipWhiteListFilter.IsAllow(new RequestPipelineData());

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsAllowTest_WithNoWhiteListDefinedAndOtherRequestHandlerReturnFalse_WillReturnFalse()
        {
            // Arrange
            var fakeSettings = Substitute.For<ISettings<string>>();
            fakeSettings.Get(WellknownConstants.WhiteListIp).Returns("");

            var fakeIpHelper = Substitute.For<IIpHelper>();
            fakeIpHelper.GetIp(null).ReturnsForAnyArgs("10.0.18.99");

            var fakeFilterFilter = Substitute.For<IRequestFilter>();
            fakeFilterFilter.IsAllow(null).ReturnsForAnyArgs(false);

            _ipWhiteListFilter = new IpWhiteListFilter(fakeSettings, fakeIpHelper, fakeFilterFilter);

            // Act
            var actual = _ipWhiteListFilter.IsAllow(new RequestPipelineData());

            // Assert
            Assert.IsFalse(actual);
        }
    }
}
