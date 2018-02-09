using NumberToWords;

namespace WordConverterLibrary
{
    /// <summary>
    /// An implementation of number converter which convert a number to an English words. For example, given 110 the output would be one hundred and ten dollar. It uses third party nuget package.
    /// </summary>
    public class ThirdPartyConverterProvider : IWordConverterProvider
    {
        /// <summary>
        /// Convert a number to a words number. 
        /// </summary>
        /// <param name="input">a non negative value decimal number</param>
        /// <returns>a words number. For example, one hundred and ten dollar</returns>
        public string Convert(decimal input)
        {
            int number = int.Parse(input.ToString("F0"));
            var rawFraction = (input - number);

            if (rawFraction == 0)
            {
                return Converter.ConvertNumbertoWords(number);
            }

            int fraction = int.Parse(rawFraction.ToString("F2").Substring(2));

            return $"{Converter.ConvertNumbertoWords(number)} and {Converter.ConvertNumbertoWords(fraction)} cents";
        }
    }
}