
using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

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
