namespace WordConverterLibrary
{
    /// <summary>
    /// interface for the underlying word converter provider that convert a number to a word. For example, from 100 to one hundred dollar.
    /// </summary>
    public interface IWordConverterProvider
    {
        string Convert(decimal input);
    }
}