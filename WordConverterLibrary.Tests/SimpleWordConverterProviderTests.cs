﻿using System;
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
        [TestCase(83100, "eighty-three thousand, one hundred")]
        [TestCase(12345678, "twelve million, three hundred forty-five thousand, six hundred seventy-eight")]
        [TestCase(3000000, "three million")]
        [TestCase(1234567890, "one billion, two hundred thirty-four million, five hundred sixty-seven thousand, eight hundred ninety")]
        [TestCase(60000000, "sixty million")]
        [TestCase(88000900, "eighty-eight million, nine hundred")]
        public void ConvertTests(decimal input, string expected)
        {
            // Arrange
            _provider = new SimpleWordConverterProvider();

            // Act
            string actual = _provider.Convert(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConvertTests_WithPrecision_WillReturnWithPrecisionWord()
        {
            decimal[] input = { 75644.85m , 75644.8546m };
            string[] expected = { "seventy-five thousand, six hundred forty-four and eighty-five" , "seventy-five thousand, six hundred forty-four and eighty-five" };
            var actual = new string[2];

            // Arrange
            _provider = new SimpleWordConverterProvider();

            // Act
            for (var counter = 0; counter < input.Length; counter++)
            {
                actual[counter] = _provider.Convert(input[counter]);
            }
            

            // Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }

        [TestCase(1, "ONE DOLLARS")]
        [TestCase(43, "FORTY-THREE DOLLARS")]
        [TestCase(123, "ONE HUNDRED AND TWENTY-THREE DOLLARS")]
        [TestCase(83123, "EIGHTY-THREE THOUSAND, ONE HUNDRED AND TWENTY-THREE DOLLARS")]
        [TestCase(83100, "EIGHTY-THREE THOUSAND, ONE HUNDRED DOLLARS")]
        [TestCase(12345678, "TWELVE MILLION, THREE HUNDRED AND FORTY-FIVE THOUSAND, SIX HUNDRED AND SEVENTY-EIGHT DOLLARS")]
        [TestCase(3000000, "THREE MILLION DOLLARS")]
        [TestCase(1234567890, "ONE BILLION, TWO HUNDRED AND THIRTY-FOUR MILLION, FIVE HUNDRED AND SIXTY-SEVEN THOUSAND, EIGHT HUNDRED AND NINETY DOLLARS")]
        [TestCase(60000000, "SIXTY MILLION DOLLARS")]
        [TestCase(88000900, "EIGHTY-EIGHT MILLION, NINE HUNDRED DOLLARS")]
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
        public void ConvertTests_WithPrecisionAndIncludeDollar_WillReturnWithPrecisionWordAndDollar()
        {
            decimal[] input = { 75644.85m, 75644.8546m };
            string[] expected = { "SEVENTY-FIVE THOUSAND, SIX HUNDRED AND FORTY-FOUR DOLLARS AND EIGHTY-FIVE CENTS", "SEVENTY-FIVE THOUSAND, SIX HUNDRED AND FORTY-FOUR DOLLARS AND EIGHTY-FIVE CENTS" };
            var actual = new string[2];

            // Arrange
            _provider = new SimpleWordConverterProvider(true, true, true);

            // Act
            for (var counter = 0; counter < input.Length; counter++)
            {
                actual[counter] = _provider.Convert(input[counter]);
            }


            // Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
        }

        public void ConvertTests_WithLargeNumberPrecisionAndIncludeDollar_WillReturnWithPrecisionWordAndDollar()
        {
            var input = 999999999999999999.99m;
            var expected =
                "NINE HUNDRED AND NINETY-NINE QUADRILLION, NINE HUNDRED AND NINETY-NINE TRILLION, NINE HUNDRED AND NINETY-NINE BILLION, " +
                "NINE HUNDRED AND NINETY-NINE MILLION, NINE HUNDRED AND NINETY-NINE THOUSAND, NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS";
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
            Assert.Throws<RangeException>(() => { _provider.Convert(input); });
        }
    }
}
