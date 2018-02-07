using System;

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

        private string ConvertToEnglishWord(long val)
        {
            if (val < 100)
            {
                return Convert_nn(val);
            }

            if (val < 1000)
            {
                return Convert_nnn(val);
            }

            for (var v = 0; v < _denom.Length; v++)
            {
                long didx = v - 1;
                long dval = long.Parse(Math.Pow(1000, v).ToString());
                if (dval > val)
                {
                    long mod = int.Parse(Math.Pow(1000, didx).ToString());
                    long l = val / mod;
                    long r = val - (l * mod);

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

        private string Convert_nn(long val)
        {
            if (val < 20)
                return _numberWords[val];
            for (long v = 0; v < _tiesWords.Length; v++)
            {
                string dcap = _tiesWords[v];
                long dval = 20 + 10 * v;
                if (dval + 10 > val)
                {
                    if ((val % 10) != 0)
                        return dcap + "-" + _numberWords[val % 10];
                    return dcap;
                }
            }

            throw new ArithmeticException("Something is wrong");
        }

        private string Convert_nnn(long val)
        {
            var word = "";
            var rem = val / 100;
            var mod = val % 100;
            if (rem > 0)
            {
                word = _numberWords[rem] + " hundred";
                if (_includeAnd)
                {
                    word += " and";
                }

                if (mod > 0)
                {
                    word = word + " ";
                }
            }

            if (mod > 0)
            {
                word = word + Convert_nn(mod);
            }

            return word;
        }

        private string ConvertToWord(decimal input)
        {
            int number = int.Parse(input.ToString("F0"));
            var rawFraction = (input - number);

            if (rawFraction == 0)
            {
                return this.Convert(number);
            }

            int fraction = int.Parse(rawFraction.ToString("F2").Substring(2));

            return $"{this.Convert(number)} and {this.Convert(fraction)} cents";
        }

        #endregion
    }
}