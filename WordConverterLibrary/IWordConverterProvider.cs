namespace WordConverterLibrary
{
    public interface IWordConverterProvider
    {
        string Convert(int input);

        string Convert(decimal input);
    }
}