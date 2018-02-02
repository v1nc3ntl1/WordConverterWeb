using NumberToWords;

namespace WordConverterLibrary
{
    public class WordConverterProvider : IWordConverterProvider
    {
        public string Convert(int input)
        {
            return Converter.ConvertNumbertoWords(input);
        }

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