using System.Web.Http;
using System.Web.Http.Results;
using NSubstitute;
using NUnit.Framework;
using WordConverterAPI.Controllers;
using WordConverterAPI.Models;
using WordConverterLibrary;

namespace WordConverterAPI.Tests.Controllers
{
    [TestFixture]
    public class WordConverterTests
    {
        [Test]
        public void Get_WithValidValue_WillReturnOk()
        {
            // Arrange
            var fakeProvider = Substitute.For<IWordConverterProvider>();
            fakeProvider.Convert(12m).Returns("twelve");
            var controller = new WordConverterController(fakeProvider);

            // Act
            IHttpActionResult result = controller.Get(12);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkNegotiatedContentResult<WordModel>;
            Assert.IsNotNull(okResult);
            
            Assert.IsNotNull(okResult.Content);
            Assert.AreEqual("twelve", okResult.Content.Word);
            Assert.AreEqual(12m, okResult.Content.Number);
        }

        [Test]
        public void Get_WithInValidValue_WillReturnBadRequest()
        {
            // Arrange
            var fakeProvider = Substitute.For<IWordConverterProvider>();
            var controller = new WordConverterController(fakeProvider);

            // Act
            IHttpActionResult result = controller.Get(-12m);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as BadRequestErrorMessageResult;
            Assert.IsNotNull(okResult);

            Assert.AreEqual(okResult.Message, "Invalid number");
        }
    }
}
