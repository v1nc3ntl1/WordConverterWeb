using System;
using NUnit.Framework;

namespace WordConverterLibrary.Tests
{
    public class SimpleWordConverterProviderTests
    {
        private SimpleWordConverterProvider _provider;
        
        [TestCase(1, "one")]
        [TestCase(43, "forty-three")]
        [TestCase(123, "one hundred twenty-three")]
        [TestCase(83123, "eighty-three thousand, one hundred twenty-three")]
        [TestCase(12345678, "twelve million, three hundred forty-five thousand, six hundred seventy-eight")]
        [TestCase(1234567890, "one billion, two hundred thirty-four million, five hundred sixty-seven thousand, eight hundred ninety")]
        public void ConvertTests(decimal input, string expected)
        {
            // Arrange
            _provider = new SimpleWordConverterProvider();

            // Act
            string actual = _provider.Convert(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(1, "ONE DOLLARS")]
        [TestCase(43, "FORTY-THREE DOLLARS")]
        [TestCase(123, "ONE HUNDRED AND TWENTY-THREE DOLLARS")]
        [TestCase(83123, "EIGHTY-THREE THOUSAND, ONE HUNDRED AND TWENTY-THREE DOLLARS")]
        [TestCase(12345678, "TWELVE MILLION, THREE HUNDRED AND FORTY-FIVE THOUSAND, SIX HUNDRED AND SEVENTY-EIGHT DOLLARS")]
        [TestCase(1234567890, "ONE BILLION, TWO HUNDRED AND THIRTY-FOUR MILLION, FIVE HUNDRED AND SIXTY-SEVEN THOUSAND, EIGHT HUNDRED AND NINETY DOLLARS")]
        public void ConvertTests_WithIncludeDollars_WillIncludeDollars(decimal input, string expected)
        {
            // Arrange
            _provider = new SimpleWordConverterProvider(true, true, true);

            // Act
            string actual = _provider.Convert(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTests_WithNegativeInput_WillThrowException()
        {
            // Arrange
            _provider = new SimpleWordConverterProvider();
            decimal input = -20.00m;

            // Act
            Assert.Throws<NotSupportedException>(() => { _provider.Convert(input); });
        }
    }
}
