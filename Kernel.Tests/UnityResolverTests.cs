
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace Kernel.Tests
{
    public class UnityResolverTests
    {
        private UnityResolver _resolver;
        private IUnityContainer _container;

        public UnityResolverTests()
        {
            _container = Substitute.For<IUnityContainer>();
            _resolver = new UnityResolver(_container);
        }

        [Test]
        public void GetServiceTests()
        {
            // Arrange
            var mockClass = new MockClass();
            _container.Resolve(typeof(IMock)).Returns(mockClass);

            // Act
            var actual = _resolver.GetService(typeof(IMock));

            // Assert
            Assert.AreEqual(mockClass, actual);
        }

        [Test]
        public void GetServicesTests()
        {
            // Arrange
            var mockClass = new MockClass();
            var container = new UnityContainer();
            container.RegisterType<IMock, MockClass>();
            var resolver = new UnityResolver(container);

            // Act
            var actual = resolver.GetServices(typeof(IMock));

            // Assert
            Assert.NotNull(actual);
        }

        [Test]
        public void BeginScopeTests()
        {
            // Arrange
            var called = false;
            _container.When(x => x.CreateChildContainer()).Do(x => called = true);

            // Act
            var actual = _resolver.BeginScope();

            Assert.IsTrue(called);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void DisposeTests()
        {
            // Arrange
            var called = false;
            _container.When(x => x.Dispose()).Do(x => called = true);

            // Act
            _resolver.Dispose();

            Assert.IsTrue(called);
        }

        public interface IMock
        {
            string Field1 { get; set; }
        }

        public class MockClass : IMock
        {
            public string Field1 { get; set; }
        }
    }
}
