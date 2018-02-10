
using Kernel;

namespace Abstraction.Kernel
{
    public interface IRequestFilter
    {
        bool IsAllow(RequestPipelineData data);
    }
}
