﻿using System;

namespace WordConverterLibrary
{
    public class SimpleWordConverterProvider : IWordConverterProvider
    {
        private readonly string[] _numberWords = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] _tiesWords = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] _denom = { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "sexdecillion", "septendecillion", "octodecillion", "novemdecillion", "vigintillion" };
        private readonly bool _includeCurrency;
        private readonly bool _includeAnd;
        private readonly bool _upperCase;

        public SimpleWordConverterProvider() { }
            
        public SimpleWordConverterProvider(bool includeCurrency, bool includeAnd, bool upperCase)
        {
            _includeCurrency = includeCurrency;
            _includeAnd = includeAnd;
            _upperCase = upperCase;
        }

        public string Convert(decimal input)
        {
            if (input < 0)
            {
                throw new NotSupportedException("Negative number not supported. Use positive number instead");
            }

            var currency = _includeCurrency ? " dollars" : "";
            var number = int.Parse(input.ToString("F0"));
            var rawFraction = input - number;

            if (rawFraction == 0)
            {
                return FormatOutput($"{ConvertToEnglishWord(number)}{currency}");
            }

            var fraction = int.Parse(rawFraction.ToString("F2").Substring(2));

            var cent = _includeCurrency ? " cents" : "";
            string output = $"{ConvertToEnglishWord(number)}{currency} and {ConvertToEnglishWord(fraction)}{cent}";

            return FormatOutput(output);
        }

        #region Private

        private string FormatOutput(string input)
        {
            return _upperCase ? input.ToUpper() : input;
        }

        private string ConvertToEnglishWord(long input)
        {
            if (input < 100)
            {
                return Convert_nn(input);
            }

            if (input < 1000)
            {
                return Convert_nnn(input);
            }

            for (var counter = 0; counter < _denom.Length; counter++)
            {
                long didx = counter - 1;
                var dval = long.Parse(Math.Pow(1000, counter).ToString());
                if (dval > input)
                {
                    long mod = int.Parse(Math.Pow(1000, didx).ToString());
                    var l = input / mod;
                    var r = input - (l * mod);

                    string ret = Convert_nnn(l) + " " + _denom[didx];

                    if (r > 0)
                    {
                        ret = ret + ", " + ConvertToEnglishWord(r);
                    }

                    return ret;
                }
            }

            throw new ArithmeticException("Something is wrong");
        }

        private string Convert_nn(long input)
        {
            if (input < 20)
                return _numberWords[input];
            for (long v = 0; v < _tiesWords.Length; v++)
            {
                string dcap = _tiesWords[v];
                long dval = 20 + 10 * v;
                if (dval + 10 > input)
                {
                    if ((input % 10) != 0)
                        return dcap + "-" + _numberWords[input % 10];
                    return dcap;
                }
            }

            throw new ArithmeticException("Something is wrong");
        }

        private string Convert_nnn(long input)
        {
            var word = "";
            var rem = input / 100;
            var mod = input % 100;
            if (rem > 0)
            {
                word = _numberWords[rem] + " hundred";

                if (mod > 0)
                {
                    if (_includeAnd)
                    {
                        word += " and";
                    }

                    word = word + " ";
                }
            }

            if (mod > 0)
            {
                word = word + Convert_nn(mod);
            }

            return word;
        }

        #endregion
    }
}