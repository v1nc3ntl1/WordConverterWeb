﻿using System.Web.Http;
using NUnit.Framework;

namespace Kernel.Tests
{
    public class WebApiHelperTests
    {
        [Test]
        public void RegisterTest_ShouldResolveUnityResolver()
        {
            // Arrange
            var config = new HttpConfiguration();

            // Act
            WebApiHelper.Register<IFake, Fake>(config);

            // Assert
            Assert.AreEqual(config.DependencyResolver.GetType(), typeof(UnityResolver));
        }

        [Test]
        public void RegisterTest_WithConstructor_ShouldResolveUnityResolver()
        {
            // Arrange
            var config = new HttpConfiguration();

            // Act
            WebApiHelper.Register<IFake, Fake>(config, true, true, true);

            // Assert
            Assert.AreEqual(config.DependencyResolver.GetType(), typeof(UnityResolver));
        }

        internal interface IFake
        {
            
        }

        internal class Fake : IFake
        {
            public Fake(bool field1, bool field2, bool field3) { }
        }
    }
}
