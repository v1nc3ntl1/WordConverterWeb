using System;

namespace WordConverterLibrary
{
    /// <summary>
    /// An implementation of number converter which convert a number to an English words. For example, given 110 the output would be one hundred and ten dollar.
    /// </summary>
    public class SimpleWordConverterProvider : IWordConverterProvider
    {
        private readonly string[] _numberWords = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] _tiesWords = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        private readonly string[] _denom = { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "sexdecillion", "septendecillion", "octodecillion", "novemdecillion", "vigintillion" };
        private readonly bool _includeCurrency;
        private readonly bool _includeAnd;
        private readonly bool _upperCase;
        private readonly decimal _max = 999999999999999999.99m;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimpleWordConverterProvider() { }
            
        /// <summary>
        /// Parameter constructor
        /// </summary>
        /// <param name="includeCurrency">flag to determine if to include Currency at the back of the word. For example, one hundred dollar</param>
        /// <param name="includeAnd">flag to determine if to include And word on the 2nd digit. For example, if it is true, 110 would be return as one hundred "and" ten</param>
        /// <param name="upperCase">flag to determine if to return the word in upper case</param>
        public SimpleWordConverterProvider(bool includeCurrency, bool includeAnd, bool upperCase)
        {
            _includeCurrency = includeCurrency;
            _includeAnd = includeAnd;
            _upperCase = upperCase;
        }

        /// <summary>
        /// Convert a number to a words number. 
        /// </summary>
        /// <param name="input">a non negative value decimal number towards 2 precision</param>
        /// <returns>a words number. For example, one hundred and ten dollar</returns>
        public string Convert(decimal input)
        {
            if (input < 0)
            {
                throw new RangeException("Negative number not supported. Use positive number instead.");
            }

            if (input > _max)
            {
                throw new MaxRangeException($"Cannot support number more than {_max}.");
            }

            var currency = _includeCurrency ? " dollars" : "";
            var number = System.Convert.ToUInt64(Math.Floor(input));
            var rawFraction = input - number;

            if (rawFraction == 0)
            {
                return FormatOutput($"{ConvertToEnglishWord(number)}{currency}");
            }

            var fraction = int.Parse(rawFraction.ToString("F2").Substring(2));

            var cent = _includeCurrency ? " cents" : "";
            string output = $"{ConvertToEnglishWord(number)}{currency} and {Convert_nn(fraction)}{cent}";

            return FormatOutput(output);
        }

        #region Private

        private string FormatOutput(string input)
        {
            return _upperCase ? input.ToUpper() : input;
        }

        private string ConvertToEnglishWord(ulong input)
        {
            if (input < 100)
            {
                return Convert_nn((long)input);
            }

            if (input < 1000)
            {
                return Convert_nnn((long)input);
            }
            
            for (var counter = 0; counter < _denom.Length; counter++)
            {
                long didx = counter - 1;
                var dval = System.Convert.ToUInt64(Math.Pow(1000, counter));
                if (dval > input)
                {
                    ulong mod = System.Convert.ToUInt64(Math.Pow(1000, didx));
                    var l = input / mod;
                    var r = input - (l * mod);

                    string ret = Convert_nnn((long)l) + " " + _denom[didx];

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