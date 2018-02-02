namespace WordConverterLibrary
{
    public class SimpleWordConverterProvider : IWordConverterProvider
    {
        public string Convert(int input)
        {
            return $"Converted: {input.ToString()}";
        }

        public string Convert(decimal input)
        {
            return $"Converted: {input.ToString()}";
        }
    }
}