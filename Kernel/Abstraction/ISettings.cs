
namespace Kernel.Abstraction
{
    public interface ISettings<T>
    {
        T Get(string name);
    }
}
